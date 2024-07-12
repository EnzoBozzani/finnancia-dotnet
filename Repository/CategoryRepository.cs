using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Mappers;
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

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return null;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            return await _context.Categories
                .Where(category => category.UserId == userId)
                .OrderBy(category => category.Name)
                .Select(category => category.ToCategoryDTO())
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(NewCategoryDTO bodyDTO, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(category => category.UserId == userId
                && (category.Color == bodyDTO.Color || category.Name == bodyDTO.Name));
        }

        public async Task<Category?> UpdateAsync(UpdateCategoryDTO bodyDTO, Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return null;
            }

            category.Color = bodyDTO.Color == null || bodyDTO.Color.Equals(string.Empty) ? category.Color : bodyDTO.Color;
            category.Name = bodyDTO.Name == null || bodyDTO.Name.Equals(string.Empty) ? category.Name : bodyDTO.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}