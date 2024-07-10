using FinnanciaCSharp.Data;
using FinnanciaCSharp.Interfaces;
using FinnanciaCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinnanciaCSharp.Repository
{
    public class HelpMessageRepository : IHelpMessageRepository
    {
        private readonly ApplicationDBContext _context;
        public HelpMessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<HelpMessage> CreateAsync(string body, User user)
        {
            var helpMessage = new HelpMessage
            {
                Body = body,
                UserEmail = user.Email!,
                UserId = user.Id
            };

            await _context.HelpMessages.AddAsync(helpMessage);
            await _context.SaveChangesAsync();

            return helpMessage;
        }

        public async Task<HelpMessage?> GetLastMessage(string userId)
        {
            var lastMessage = await _context.HelpMessages
                .Where(message => message.UserId == userId)
                .OrderByDescending(message => message.CreatedAt)
                .FirstOrDefaultAsync();

            return lastMessage;
        }
    }
}