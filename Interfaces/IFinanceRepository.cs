using FinnanciaCSharp.DTOs.Finance;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IFinanceRepository
    {
        Task<Finance> CreateAsync(Finance finance);
        Task<List<Finance>> GetPaginatedFinancesAsync(Guid sheetId, GetPaginatedFinancesQueryDTO queryDTO);
        Task<decimal> GetFinancesAmountAsync(Guid sheetId, string? title);
        Task<int> GetFinancesCountAsync(Guid sheetId, string? title);
        Task<List<FinanceWithCategoryDTO>> GetFinancesWithCategoriesAsync(Guid sheetId, string userId);
        Task<Finance?> UpdateAsync(User user, Guid id, EditFinanceDTO bodyDTO);
        Task<Finance?> DeleteAsync(User user, Guid id);
    }
}