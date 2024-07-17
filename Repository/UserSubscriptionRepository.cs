using FinnanciaCSharp.Data;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
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
        public async Task<UserSubscription?> GetUserSubscriptionAsync(string userId)
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
        }
    }
}