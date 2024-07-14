using FinnanciaCSharp.Data;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDBContext _context;
        public MessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Message> CreateAsync(string body, string userId, string role)
        {
            var message = new Message
            {
                Body = body,
                Role = role,
                UserId = userId
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<Message>> GetMessagesAsync(string userId)
        {
            return await _context.Messages
                .Where(message => message.UserId == userId)
                .ToListAsync();
        }
    }
}