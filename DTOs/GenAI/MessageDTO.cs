namespace FinnanciaCSharp.DTOs.GenAI
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}