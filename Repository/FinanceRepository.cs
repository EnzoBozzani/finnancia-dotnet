using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Lib;
using FinnanciaCSharp.Mappers;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        public FinanceRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Finance> CreateAsync(Finance finance)
        {
            await _context.Finances.AddAsync(finance);
            await _context.SaveChangesAsync();

            return finance;
        }

        public async Task<List<Finance>> GetPaginatedFinancesAsync(Guid sheetId, GetPaginatedFinancesQueryDTO queryDTO)
        {
            var title = queryDTO.Title ?? "";
            var skip = (queryDTO.Page - 1) * 8;

            var finances = _context.Finances.AsQueryable()
                .Include(finance => finance.Category)
                .Where(finance =>
                    finance.SheetId == sheetId
                    &&
                    (finance.Title.ToLower().Contains(title.ToLower()) ||
                        (finance.Category != null && finance.Category.Name.ToLower().Contains(title.ToLower()))
                    )
                )
                .OrderByDescending(finance => finance.CreatedAt)
                .OrderByDescending(finance => finance.Order);

            return await finances.Skip(skip).Take(8).ToListAsync();
        }

        public async Task<decimal> GetFinancesAmountAsync(Guid sheetId, string? title)
        {
            var formattedTitle = title ?? "";

            var profitAmount = await _context.Finances
                .Where(finance => finance.SheetId == sheetId && finance.Type == "PROFIT"
                    &&
                    (
                        finance.Title.ToLower().Contains(formattedTitle.ToLower())
                        ||
                        finance.Category != null && finance.Category.Name.ToLower().Contains(formattedTitle.ToLower())
                    )
                )
                .SumAsync(finance => finance.Amount);

            var expenseAmount = await _context.Finances
                .Where(finance => finance.SheetId == sheetId && finance.Type == "EXPENSE"
                    &&
                    (
                        finance.Title.ToLower().Contains(formattedTitle.ToLower())
                        ||
                        finance.Category != null && finance.Category.Name.ToLower().Contains(formattedTitle.ToLower())
                    )
                )
                .SumAsync(finance => finance.Amount);

            return profitAmount - expenseAmount;
        }

        public async Task<int> GetFinancesCountAsync(Guid sheetId, string? title)
        {
            var formattedTitle = title ?? "";

            var financesCount = await _context.Finances
                .Where(finance => finance.SheetId == sheetId
                    &&
                    (
                        finance.Title.ToLower().Contains(formattedTitle.ToLower())
                        ||
                        finance.Category != null && finance.Category.Name.ToLower().Contains(formattedTitle.ToLower())
                    )
                )
                .CountAsync();

            return financesCount;
        }

        public async Task<List<FinanceWithCategoryDTO>> GetFinancesWithCategoriesAsync(Guid sheetId, string userId)
        {
            var finances = _context.Finances.AsQueryable()
                .Where(finance => finance.SheetId == sheetId && finance.Sheet!.UserId == userId)
                .Include(finance => finance.Category)
                .Select(finance => finance.ToFinanceWithCategoryDTO());

            return await finances.ToListAsync();
        }

        public async Task<Finance?> UpdateAsync(User user, Guid id, EditFinanceDTO bodyDTO)
        {
            var financeToBeEdited = await _context.Finances
                .FirstOrDefaultAsync(finance => finance.Id == id && finance.Sheet!.UserId == user.Id);

            if (financeToBeEdited == null)
            {
                return null;
            }

            var sheet = await _context.Sheets.
                FirstOrDefaultAsync(sheet => sheet.Id == financeToBeEdited.SheetId && sheet.UserId == user.Id);

            if (sheet == null)
            {
                return null;
            }

            if (bodyDTO.Date != null)
            {
                var financeMonth = int.Parse(bodyDTO.Date.Substring(3, 2));
                var financeYear = bodyDTO.Date.Substring(6);

                var sheetMonth = sheet.Name.Split("/")[0];
                var sheetYear = sheet.Name.Split("/")[1];

                var monthMap = Utils.MonthMap();

                if (!monthMap[financeMonth].Equals(sheetMonth) || !financeYear.Equals(sheetYear))
                {
                    return null;
                }
            }

            if (bodyDTO.Amount == null && bodyDTO.FinanceType != null && bodyDTO.FinanceType != financeToBeEdited.Type)
            {
                sheet.TotalAmount = bodyDTO.FinanceType == "PROFIT" ?
                    sheet.TotalAmount + 2 * financeToBeEdited.Amount
                    :
                    sheet.TotalAmount - 2 * financeToBeEdited.Amount;

                user.TotalAmount = bodyDTO.FinanceType == "PROFIT" ?
                    user.TotalAmount + 2 * financeToBeEdited.Amount
                    :
                    user.TotalAmount - 2 * financeToBeEdited.Amount;
            }

            if (bodyDTO.Amount != null)
            {
                var newAmount = bodyDTO.Amount ?? 0;
                if (bodyDTO.FinanceType != null && bodyDTO.FinanceType != financeToBeEdited.Type)
                {
                    sheet.TotalAmount = bodyDTO.FinanceType == "PROFIT" ?
                        sheet.TotalAmount + financeToBeEdited.Amount + newAmount
                        :
                        sheet.TotalAmount - financeToBeEdited.Amount - newAmount;

                    user.TotalAmount = bodyDTO.FinanceType == "PROFIT" ?
                        user.TotalAmount + financeToBeEdited.Amount + newAmount
                        :
                        user.TotalAmount - financeToBeEdited.Amount - newAmount;
                }
                else
                {
                    sheet.TotalAmount = financeToBeEdited.Type == "PROFIT" ?
                        sheet.TotalAmount - financeToBeEdited.Amount + newAmount
                        :
                        sheet.TotalAmount + financeToBeEdited.Amount - newAmount;

                    user.TotalAmount = financeToBeEdited.Type == "PROFIT" ?
                        user.TotalAmount - financeToBeEdited.Amount + newAmount
                        :
                        user.TotalAmount + financeToBeEdited.Amount - newAmount;
                }
            }

            financeToBeEdited.Title = bodyDTO.Title ?? financeToBeEdited.Title;
            financeToBeEdited.Amount = bodyDTO.Amount ?? financeToBeEdited.Amount;
            financeToBeEdited.Type = bodyDTO.FinanceType ?? financeToBeEdited.Type;
            financeToBeEdited.Date = bodyDTO.Date ?? financeToBeEdited.Date;
            financeToBeEdited.Order = bodyDTO.Date != null ? int.Parse(bodyDTO.Date.Substring(0, 2)) : financeToBeEdited.Order;
            financeToBeEdited.CategoryId = bodyDTO.CategoryId != null ?
                bodyDTO.CategoryId.Equals("null") ? null : bodyDTO.CategoryId
                : financeToBeEdited.CategoryId;

            _context.Finances.Update(financeToBeEdited);
            _context.Sheets.Update(sheet);
            await _context.SaveChangesAsync();

            await _userManager.UpdateAsync(user);

            return financeToBeEdited;
        }

        public async Task<Finance?> DeleteAsync(User user, Guid id)
        {
            var financeToBeDeleted = await _context.Finances
                .FirstOrDefaultAsync(finance => finance.Id == id && finance.Sheet!.UserId == user.Id);

            if (financeToBeDeleted == null)
            {
                return null;
            }

            var sheet = await _context.Sheets
                .FirstOrDefaultAsync(sheet => sheet.Id == financeToBeDeleted.SheetId && sheet.UserId == user.Id);

            if (sheet == null)
            {
                return null;
            }

            sheet.TotalAmount = financeToBeDeleted.Type == "PROFIT" ?
                sheet.TotalAmount - financeToBeDeleted.Amount
                :
                sheet.TotalAmount + financeToBeDeleted.Amount;

            sheet.FinancesCount -= 1;

            user.TotalAmount = financeToBeDeleted.Type == "PROFIT" ?
                user.TotalAmount - financeToBeDeleted.Amount
                :
                user.TotalAmount + financeToBeDeleted.Amount;

            _context.Finances.Remove(financeToBeDeleted);
            _context.Sheets.Update(sheet);
            await _context.SaveChangesAsync();

            await _userManager.UpdateAsync(user);

            return financeToBeDeleted;
        }
    }
}