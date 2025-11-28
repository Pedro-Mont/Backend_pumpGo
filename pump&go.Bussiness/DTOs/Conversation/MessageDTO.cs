namespace pump_go.pump_go.Bussiness.DTOs.Conversation
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
    }
}
