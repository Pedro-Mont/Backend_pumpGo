using pump_go.Entities;

namespace pump_go.Interfaces.IRepositories
{
    public interface ISignatureRepository
    {
        Task<Signature> GetByIdAsync(Guid id);
        Task<Signature> GetByUserIdAsync(Guid userId);
        Task AddAsync(Signature signature);
        Task UpdateAsync(Signature signature);
    }
}
