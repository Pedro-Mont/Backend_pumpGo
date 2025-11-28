using pump_go.Entities;
//using System.Text.RegularExpressions;//

namespace pump_go.Interfaces.IRepositories
{
    public interface IMuscleGroupRepository
    {
        Task<MuscleGroup> GetByIdAsync(Guid id);
        Task<IEnumerable<MuscleGroup>> GetAllAsync();
        Task AddAsync(MuscleGroup group);
        Task UpdateAsync(MuscleGroup group);
    }
}
