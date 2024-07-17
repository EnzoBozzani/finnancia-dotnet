using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscription?> GetUserSubscriptionAsync(string userId);
    }
}