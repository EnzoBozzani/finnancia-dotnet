using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Mappers;
using FinnanciaCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly ApplicationDBContext _context;
        public FinanceRepository(ApplicationDBContext context)
        {
            _context = context;
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

        public async Task<List<FinanceWithCategoryDTO>> GetFinancesWithCategories(Guid sheetId, string userId)
        {
            var finances = _context.Finances.AsQueryable()
                .Where(finance => finance.SheetId == sheetId && finance.Sheet!.UserId == userId)
                .Include(finance => finance.Category)
                .Select(finance => finance.ToFinanceWithCategoryDTO());

            return await finances.ToListAsync();
        }
    }
}