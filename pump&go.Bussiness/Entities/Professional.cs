using pump_go.Entities.Enums;

namespace pump_go.Entities
{
    public class Professional
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public ProfessionalType Specialty { get; set; }
    }
}
