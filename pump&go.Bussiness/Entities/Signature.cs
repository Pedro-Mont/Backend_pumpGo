using pump_go.Entities.Enums;

namespace pump_go.Entities
{
    public class Signature
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public SignatureStatus Status { get; set; }
    }
}
