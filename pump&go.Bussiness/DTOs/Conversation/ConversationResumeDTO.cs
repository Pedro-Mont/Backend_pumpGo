namespace pump_go.pump_go.Bussiness.DTOs.Conversation
{
    public class ConversationResumeDTO
    {
        public Guid Id { get; set; }
        public string ProfessionalName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageDate { get; set; }
    }
}
