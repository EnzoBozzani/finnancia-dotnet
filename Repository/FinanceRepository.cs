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

        public async Task<List<Finance>> GetPaginatedFinances(Guid sheetId, GetPaginatedFinancesQueryDTO queryDTO)
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
                .OrderByDescending(finance => finance.Order)
                .OrderByDescending(finance => finance.CreatedAt);

            return await finances.Skip(skip).Take(8).ToListAsync();
        }
    }
}