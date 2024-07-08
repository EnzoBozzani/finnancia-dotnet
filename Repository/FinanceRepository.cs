using FinnanciaCSharp.Data;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;

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
    }
}