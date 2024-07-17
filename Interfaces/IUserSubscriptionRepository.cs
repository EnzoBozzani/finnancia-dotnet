using FinnanciaCSharp.DTOs.UserSubscription;

namespace FinnanciaCSharp.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscriptionDTO?> GetUserSubscriptionAsync(string userId);
    }
}