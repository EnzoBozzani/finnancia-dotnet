namespace FinnanciaCSharp.DTOs.Category
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "transparent";
        public string UserId { get; set; } = string.Empty;
    }
}