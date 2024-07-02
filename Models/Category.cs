using System.ComponentModel.DataAnnotations.Schema;
using FinnanciaCSharp.Enums;

namespace FinnanciaCSharp.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.transparent;
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}