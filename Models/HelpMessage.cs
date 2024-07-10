using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinnanciaCSharp.Models
{
    public class HelpMessage
    {
        [Key]
        public Guid Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}