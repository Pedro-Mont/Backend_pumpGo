using pump_go.Entities.Enums;
using pump_go.pump_go.Bussiness.DTOs.Signature;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface ISignatureService
    {
        Task<SignatureDTO> GetUsersSignatureAsync(Guid userId);
        Task<bool> UserHasAccessToProfessional(Guid userId, ProfessionalType type);
        Task<SignatureDTO> UpgradeSignatureAsync(Guid userId, string PlanName);
    }
}
