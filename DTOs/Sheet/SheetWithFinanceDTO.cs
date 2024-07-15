using FinnanciaCSharp.DTOs.Finance;

namespace FinnanciaCSharp.DTOs.Sheet
{
    public class SheetWithFinanceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int Order { get; set; }
        public int FinancesCount { get; set; } = 0;
        public IEnumerable<FinanceDTO> Finances { get; set; } = new List<FinanceDTO>();
    }
}