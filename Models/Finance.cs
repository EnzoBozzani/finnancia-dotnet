using System.ComponentModel.DataAnnotations.Schema;

namespace FinnanciaCSharp.Models
{
    public class Finance
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Date { get; set; } = string.Empty;
        [ForeignKey("Sheet")]
        public Guid? SheetId { get; set; }
        public Sheet? Sheet { get; set; }
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}