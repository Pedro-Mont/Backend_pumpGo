namespace pump_go.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProfessionalId { get; set; }
        public User User { get; set; }
        public Professional Professional { get; set; }
        public DateTime StartDate { get; set; }
        public List<Message> Messages { get; set; }
    }
}
