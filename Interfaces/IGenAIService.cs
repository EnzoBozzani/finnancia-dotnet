using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Interfaces
{
    public interface IGenAIService
    {
        Task<string?> ChatWithAIAsync(string message, List<Message> oldMessages, string username);
    }
}