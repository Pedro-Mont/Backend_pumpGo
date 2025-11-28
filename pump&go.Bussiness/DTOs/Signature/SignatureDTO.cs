using pump_go.Entities.Enums;

namespace pump_go.pump_go.Bussiness.DTOs.Signature
{
    public class SignatureDTO
    {
        public string PlanName {get; set;}
        public DateTime ExpirationDate { get; set; }
        public SignatureStatus Status { get; set; }
    }
}
