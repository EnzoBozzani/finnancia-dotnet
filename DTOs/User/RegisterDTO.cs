using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.User
{
    public class RegisterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}