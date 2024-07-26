using backend.Model.Domain.User;

namespace backend.Model.Domain.Chat
{
    public class UserConversation
    {
        public string UserId { get; set; }
        public UserDetails User { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
