namespace FinnanciaCSharp.DTOs
{
    public class SheetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Order { get; set; }
        public int FinancesCount { get; set; } = 0;
    }
}