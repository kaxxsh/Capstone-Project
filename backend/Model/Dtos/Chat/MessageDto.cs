namespace backend.Model.Dtos.Chat
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
