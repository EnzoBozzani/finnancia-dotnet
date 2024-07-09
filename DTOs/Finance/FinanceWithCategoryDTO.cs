using FinnanciaCSharp.DTOs.Category;

namespace FinnanciaCSharp.DTOs.Finance
{
    public class FinanceWithCategoryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Date { get; set; } = string.Empty;
        public Guid? SheetId { get; set; }
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public CategoryDTO? Category { get; set; }
    }
}