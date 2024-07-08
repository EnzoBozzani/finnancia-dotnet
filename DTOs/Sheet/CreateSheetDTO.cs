using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.Sheet
{
    public class CreateSheetDTO
    {
        [Required]
        [Range(1, 12, ErrorMessage = "O mês deve estar entre 1 e 12")]
        public int Month { get; set; }
        [Required]
        [Range(0000, 9999, ErrorMessage = "O ano deve estar entre 0000 e 9999")]
        public int Year { get; set; }
    }
}