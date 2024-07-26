using backend.Model.Domain.User;
using Microsoft.VisualBasic;

namespace backend.Model.Domain.Chat
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public UserDetails Sender { get; set; }
        public Conversation Conversation { get; set; }
    }
}
