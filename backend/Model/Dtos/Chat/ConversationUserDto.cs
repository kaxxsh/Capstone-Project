namespace backend.Model.Dtos.Chat
{
    public class ConversationUserDto
    {
        public Guid ConversationId { get; set; }
        public string User1Name { get; set; }
        public string User2Name { get; set; }
    }
}
