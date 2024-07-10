using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategory(NewCategoryDTO bodyDTO, string userId);
        Task<Category> CreateAsync(NewCategoryDTO bodyDTO, string userId);
        Task<List<CategoryDTO>> GetCategories(string userId);
    }
}