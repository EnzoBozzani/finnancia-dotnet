using System.ComponentModel.DataAnnotations.Schema;

namespace FinnanciaCSharp.Models
{
    public class UserSubscription
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        [Column("stripe_customer_id")]
        public string StripeCustomerId { get; set; } = string.Empty;
        [Column("stripe_subscription_id")]
        public string StripeSubscriptionId { get; set; } = string.Empty;
        [Column("stripe_price_id")]
        public string StripePriceId { get; set; } = string.Empty;
        [Column("stripe_current_period_end")]
        public DateTime StripeCurrentPeriodEnd { get; set; }
    }
}