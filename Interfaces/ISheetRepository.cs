using FinnanciaCSharp.DTOs;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ISheetRepository
    {
        Task<Sheet?> GetSheetByIdAsync(Guid id);
        Task<Sheet> CreateAsync(Sheet sheet);
        Task<bool> SheetExistsByMonthAndYear(int month, int year, Guid userId);
        Task<List<SheetDTO>> GetSheetsByUserIdAsync(Guid userId);
    }
}