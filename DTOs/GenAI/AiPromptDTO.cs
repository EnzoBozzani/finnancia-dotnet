using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.DTOs.GenAI
{
    public class AiPromptDTO
    {
        [Required]
        [Length(1, 500)]
        public string Message { get; set; } = string.Empty;
    }
}