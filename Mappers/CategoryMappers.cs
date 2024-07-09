using FinnanciaCSharp.DTOs.Category;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDTO ToCategoryDTO(this Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                UserId = category.UserId
            };
        }
    }
}