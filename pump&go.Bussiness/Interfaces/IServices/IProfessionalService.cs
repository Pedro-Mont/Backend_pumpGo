using pump_go.Entities.Enums;
using pump_go.pump_go.Bussiness.DTOs.Professional;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface IProfessionalService
    {
        Task<IEnumerable<ProfessionalResumeDTO>> GetProfessionalAsync(ProfessionalType specialty);
        Task<DetailedProfessionalDTO> GetProfessionalProfileAsync(Guid professionalId);

    }
}
