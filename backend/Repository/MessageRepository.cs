using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Chat;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Include(m => m.Sender)
                .ToListAsync();
        }
    }
}
