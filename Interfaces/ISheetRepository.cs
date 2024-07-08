using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ISheetRepository
    {
        Task<Sheet?> GetSheetByIdAsync(Guid id);
        Task<Sheet> CreateAsync(Sheet sheet);
        Task<bool> SheetExistsByMonthAndYearAsync(int month, int year, string userId);
        Task<List<SheetDTO>> GetSheetsByUserIdAsync(string userId);
        Task<Sheet?> DeleteSheetAsync(Guid id);
    }
}