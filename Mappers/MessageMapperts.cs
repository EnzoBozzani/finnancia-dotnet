using FinnanciaCSharp.DTOs.GenAI;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Mappers
{
    public static class MessageMapperts
    {
        public static MessageDTO ToMessageDTO(this Message message)
        {
            return new MessageDTO
            {
                Id = message.Id,
                Body = message.Body,
                CreatedAt = message.CreatedAt,
                Role = message.Role,
                UserId = message.UserId,
            };
        }
    }
}