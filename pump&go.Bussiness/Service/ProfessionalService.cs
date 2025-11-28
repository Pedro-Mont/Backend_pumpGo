using Microsoft.AspNetCore.Razor.Hosting;
using pump_go.Entities.Enums;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.Professional;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IServices;

namespace pump_go.pump_go.Bussiness.Service
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository _professionalRepository;

        public ProfessionalService(IProfessionalRepository professionalRepository)
        {
            _professionalRepository = professionalRepository;
        }

        public async Task<IEnumerable<ProfessionalResumeDTO>> GetProfessionalAsync(ProfessionalType speciality)
        {
            var professionals = await _professionalRepository.GetBySpecialtyAsync(speciality);

            return professionals.Select(p => new ProfessionalResumeDTO
            {
                Id = p.Id,
                Name = p.Name,
                Specialty = p.Specialty.ToString()
            });
        }

    public async Task<DetailedProfessionalDTO> GetProfessionalProfileAsync(Guid professionalId)
        {
            var professional = await _professionalRepository.GetByIdAsync(professionalId);

            if (professional == null)
            {
                throw new NotFoundException("Profissional não encontrado.");
            }

            return new DetailedProfessionalDTO
            {
                Id = professional.Id,
                Name = professional.Name,
                Biography = professional.Biography,
                Specialty = professional.Specialty.ToString()
            };
        }
    }
}
