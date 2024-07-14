using Microsoft.AspNetCore.Identity;

namespace FinnanciaCSharp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = 0;
        public bool IsInitialAmountSet { get; set; } = false;
        public bool HasUsedFreeReport { get; set; } = false;
        public List<Sheet> Sheets { get; set; } = new List<Sheet>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<HelpMessage> HelpMessages { get; set; } = new List<HelpMessage>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
