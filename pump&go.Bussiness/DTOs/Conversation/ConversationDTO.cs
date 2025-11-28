namespace pump_go.pump_go.Bussiness.DTOs.Conversation
{
    public class ConversationDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string ProfessionalName { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
