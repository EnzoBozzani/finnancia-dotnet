using FinnanciaCSharp.Data;
using FinnanciaCSharp.DTOs.UserSubscription;
using FinnanciaCSharp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly ApplicationDBContext _context;
        public UserSubscriptionRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserSubscriptionDTO?> GetUserSubscriptionAsync(string userId)
        {
            var userSubscription = await _context.UsersSubscriptions
                .Where(userSub => userSub.UserId == userId)
                .FirstOrDefaultAsync();

            if (userSubscription == null)
            {
                return null;
            }

            var isActive =
                userSubscription.StripePriceId != null && userSubscription.StripeCurrentPeriodEnd.AddDays(1) > DateTime.Now;

            return new UserSubscriptionDTO
            {
                Id = userSubscription.Id,
                UserId = userSubscription.UserId,
                StripeSubscriptionId = userSubscription.StripeSubscriptionId,
                StripeCurrentPeriodEnd = userSubscription.StripeCurrentPeriodEnd,
                StripeCustomerId = userSubscription.StripeCustomerId,
                StripePriceId = userSubscription.StripePriceId ?? "",
                IsActive = isActive
            };
        }
    }
}