using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.User
{
    public class NewHelpMessageDTO
    {
        [Required]
        [Length(5, 250)]
        public string Message { get; set; } = string.Empty;
    }
}