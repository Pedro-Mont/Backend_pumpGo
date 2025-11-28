using pump_go.Entities;

namespace pump_go.Interfaces.IRepositories
{
    public interface IRoutineRepository
    {
        Task<Routine> GetByIdAsync(Guid id);
        Task<IEnumerable<Routine>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Routine>> GetPublicsAsync();
        Task AddAsync (Routine routine);
        Task UpdateAsync(Routine routine);
        Task DeleteAsync (Guid id);
    }
}
