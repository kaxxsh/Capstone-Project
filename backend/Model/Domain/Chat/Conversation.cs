namespace backend.Model.Domain.Chat
{
    public class Conversation
    {
        public Guid ConversationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
    }
}
