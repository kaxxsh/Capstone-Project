using backend.Model.Dtos.Chat;

namespace backend.Interface.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string senderId, Guid conversationId, string content);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid conversationId);
        Task<IEnumerable<ConversationDto>> GetConversationsForUserAsync(string userId);
        Task<ConversationDto> FindOrCreateConversationAsync(string ChatWith);
        Task<IEnumerable<ConversationUserDto>> GetAllConversationsAsync();
    }
}
