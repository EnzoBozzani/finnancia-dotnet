using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.Finance
{
    public class CreateFinanceDTO
    {
        [Required]
        [Range(0.01, 1E12)]
        public decimal Amount { get; set; }
        [Required]
        [Length(10, 10, ErrorMessage = "A data tem que ter 10 caracteres")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/(\d{4})$", ErrorMessage = "Formato esperado: DD/MM/YYYY")]
        public string Date { get; set; } = string.Empty;
        [Required]
        [Length(2, 50, ErrorMessage = "O título deve ter entre 2 e 50 caracteres")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(PROFIT|EXPENSE)$", ErrorMessage = "Tipo inválido")]
        public string FinanceType { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; }
    }
}