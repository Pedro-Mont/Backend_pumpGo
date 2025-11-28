namespace pump_go.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid Sender { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
    }
}
