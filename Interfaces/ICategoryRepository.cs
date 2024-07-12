using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryAsync(NewCategoryDTO bodyDTO, string userId);
        Task<Category> CreateAsync(NewCategoryDTO bodyDTO, string userId);
        Task<List<CategoryDTO>> GetCategoriesAsync(string userId);
        Task<Category?> UpdateAsync(UpdateCategoryDTO bodyDTO, Guid id);
    }
}