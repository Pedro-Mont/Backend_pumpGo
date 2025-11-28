namespace pump_go.pump_go.Bussiness.DTOs.Conversation
{
    public class NewMessageDTO
    {
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
    }
}
