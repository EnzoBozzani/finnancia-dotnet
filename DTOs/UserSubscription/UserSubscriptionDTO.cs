namespace FinnanciaCSharp.DTOs.UserSubscription
{
    public class UserSubscriptionDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string StripeCustomerId { get; set; } = string.Empty;
        public string StripeSubscriptionId { get; set; } = string.Empty;
        public string StripePriceId { get; set; } = string.Empty;
        public DateTime StripeCurrentPeriodEnd { get; set; }
        public bool IsActive { get; set; }
    }
}