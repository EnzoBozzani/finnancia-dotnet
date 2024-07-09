using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IFinanceRepository
    {
        Task<Finance> CreateAsync(Finance finance);
        Task<List<Finance>> GetPaginatedFinances(Guid sheetId, GetPaginatedFinancesQueryDTO queryDTO);
    }
}