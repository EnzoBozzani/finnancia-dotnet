using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IHelpMessageRepository
    {
        Task<HelpMessage> CreateAsync(string body, User user);
        Task<HelpMessage?> GetLastMessageAsync(string userId);
    }
}