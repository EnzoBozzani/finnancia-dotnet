using System.ComponentModel.DataAnnotations;

namespace FinnanciaCSharp.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime EmailVerified { get; set; }
        public string? Image { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public bool IsInitialAmountSet { get; set; } = false;
        public bool HasUsedFreeReport { get; set; } = false;
        public List<Sheet> Sheets { get; set; } = new List<Sheet>();
    }
}

//   accounts Account[]
//   sheets Sheet[]
//   messages Message[]
//   categories Category[]
//   helpMessages HelpMessage[]