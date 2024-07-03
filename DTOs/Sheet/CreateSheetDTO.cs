namespace FinnanciaCSharp.DTOs.Sheet
{
    public class CreateSheetDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid UserId { get; set; }
    }
}