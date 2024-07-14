using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetMessagesAsync(string userId);
        Task<Message> CreateAsync(string body, string userId, string role);
    }
}