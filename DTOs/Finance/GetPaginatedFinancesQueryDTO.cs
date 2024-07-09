namespace FinnanciaCSharp.DTOs.Finance
{
    public class GetPaginatedFinancesQueryDTO
    {
        public int Page { get; set; } = 1;
        public string? Title { get; set; } = null;
    }
}