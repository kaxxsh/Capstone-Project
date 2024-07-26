using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Chat;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Repository
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task<Conversation> GetConversationByUsersAsync(string userId1, string userId2)
        {
            return await _context.UserConversations
                .Where(uc => (uc.UserId == userId1 || uc.UserId == userId2))
                .GroupBy(uc => uc.ConversationId)
                .Where(g => g.Count() == 2)
                .Select(g => g.FirstOrDefault().Conversation)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId)
        {
            return await _context.UserConversations
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Conversation)
                    .ThenInclude(c => c.Messages)
                .Include(uc => uc.Conversation)
                    .ThenInclude(c => c.UserConversations)
                        .ThenInclude(uc => uc.User) 
                .Select(uc => uc.Conversation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conversation>> GetAllConversation()
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.UserConversations)
                    .ThenInclude(uc => uc.User)
                .ToListAsync();
        }

    }
}
