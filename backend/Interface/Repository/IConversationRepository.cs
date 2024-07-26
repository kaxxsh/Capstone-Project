using backend.Model.Domain.Chat;

namespace backend.Interface.Repository
{
    public interface IConversationRepository
    {
        Task AddConversationAsync(Conversation conversation);
        Task<Conversation> GetConversationByUsersAsync(string userId1, string userId2);
        Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(string userId);
        Task<IEnumerable<Conversation>> GetAllConversation();
    }
}
