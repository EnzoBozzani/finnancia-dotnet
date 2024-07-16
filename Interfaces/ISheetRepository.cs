using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.DTOs.Sheet;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ISheetRepository
    {
        Task<Sheet?> GetSheetByIdAsync(Guid id, string userId);
        Task<Sheet> CreateAsync(Sheet sheet);
        Task<bool> SheetExistsByMonthAndYearAsync(int month, int year, string userId);
        Task<List<SheetDTO>> GetSheetsByUserIdAsync(string userId);
        Task<Sheet?> DeleteSheetAsync(Guid id);
        Task<bool> UpdateTotalAmountAndFinancesCountAsync(Guid sheetId, Finance finance);
        Task<List<SheetWithFinanceDTO>> GetSheetWithFinances(string userId);
        Task<SheetWithFinanceDTO?> GetSheetWith8FirstFinances(string userId, Guid sheetId);
    }
}