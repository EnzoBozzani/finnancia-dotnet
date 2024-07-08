using System.ComponentModel.DataAnnotations.Schema;

namespace FinnanciaCSharp.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "transparent";
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}