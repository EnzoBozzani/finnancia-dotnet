using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.User
{
    public class ChangeAmountDTO
    {
        [Required]
        [Range(-1E12, 1E12)]
        public decimal Amount { get; set; }
    }
}