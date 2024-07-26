namespace backend.Model.Dtos.Chat
{
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string User1Name { get; set; }
        public string User2Name { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
