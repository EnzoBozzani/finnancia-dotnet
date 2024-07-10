using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(NewCategoryDTO bodyDTO, string userId)
        {
            var category = new Category
            {
                Name = bodyDTO.Name,
                Color = bodyDTO.Color,
                UserId = userId
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> GetCategory(NewCategoryDTO bodyDTO, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(category => category.UserId == userId
                && (category.Color == bodyDTO.Color || category.Name == bodyDTO.Name));
        }
    }
}