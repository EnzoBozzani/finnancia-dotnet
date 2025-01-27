using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinnanciaCSharp.Models
{
    public class Sheet
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [ForeignKey("User")]
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Order { get; set; }
        public int FinancesCount { get; set; } = 0;
        public List<Finance> Finances { get; set; } = new List<Finance>();
        public User User { get; set; }
    }
}