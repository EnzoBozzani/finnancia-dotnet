using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.Category
{
    public class NewCategoryDTO
    {
        [Required]
        [Length(1, 60)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(transparent|red|orange|amber|yellow|lime|green|emerald|teal|cyan|sky|blue|indigo|violet|purple|fuchsia|pink|rose)$", ErrorMessage = "Cor inválida")]
        public string Color { get; set; } = string.Empty;
    }
}