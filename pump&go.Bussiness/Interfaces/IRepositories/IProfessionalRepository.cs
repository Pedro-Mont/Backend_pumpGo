using pump_go.Entities;
using pump_go.Entities.Enums;

namespace pump_go.Interfaces.IRepositories
{
    public interface IProfessionalRepository
    {
        Task<Professional> GetByIdAsync(Guid Id);
        Task<IEnumerable<Professional>> GetBySpecialtyAsync(ProfessionalType speciality);
        Task<IEnumerable<Professional>> GetAllAsync();
        Task AddAsync(Professional professional);
    }
}
