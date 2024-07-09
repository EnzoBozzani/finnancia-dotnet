using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IFinanceRepository
    {
        Task<Finance> CreateAsync(Finance finance);
        Task<List<Finance>> GetPaginatedFinances(Guid sheetId, GetPaginatedFinancesQueryDTO queryDTO);
        Task<decimal> GetFinancesAmount(Guid sheetId, string? title);
        Task<int> GetFinancesCount(Guid sheetId, string? title);
    }
}