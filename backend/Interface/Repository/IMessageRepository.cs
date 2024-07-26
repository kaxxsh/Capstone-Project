using backend.Model.Domain.Chat;

namespace backend.Interface.Repository
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(Guid conversationId);
    }
}
