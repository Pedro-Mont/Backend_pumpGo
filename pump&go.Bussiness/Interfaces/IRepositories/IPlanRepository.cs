using pump_go.Entities;

namespace pump_go.pump_go.Bussiness.Interfaces.IRepositories
{
    public interface IPlanRepository
    {
        Task<Plan> GetByIdAsync(int id);
        Task<Plan> GetByNameAsync(string name);
        Task<IEnumerable<Plan>> GetAllAsync();
        Task AddAsync(Plan plan);
        Task UpdateAsync(Plan plan);
    }
}
