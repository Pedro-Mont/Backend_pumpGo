using pump_go.Entities;
using System;

namespace pump_go.Interfaces.IRepositories
{
    public interface IConversationRepository
    {
        Task<Conversation> GetByIdAsync(Guid id);
        Task<IEnumerable<Conversation>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Conversation>> GetByProfessionalIdAsync(Guid professionalId);
        Task<Conversation> FindByUserAndProfessionalAsync(Guid userId, Guid professionalId);
        Task AddAsync(Conversation conversation);
        Task UpdateAsync(Conversation conversation);
        Task AddMessageAsync(Message message);
    }
}
