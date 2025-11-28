using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;
using System;

namespace pump_go.pump_go.Data.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> GetByIdAsync(Guid id)
        {
            return await _context.Conversas.Include(c => c.Messages).Include(c => c.User).Include(c => c.Professional).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Conversation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Conversas.Where(c => c.UserId == userId).Include(c => c.Professional).OrderByDescending(c => c.StartDate).ToArrayAsync();
        }

        public async Task<IEnumerable<Conversation>> GetByProfessionalIdAsync(Guid professionalId)
        {
            return await _context.Conversas.Where(c => c.ProfessionalId == professionalId).OrderByDescending(c => c.StartDate).ToListAsync();
        }

        public async Task<Conversation> FindByUserAndProfessionalAsync(Guid userId, Guid professionalId)
        {
            return await _context.Conversas.Include(c => c.Messages).FirstOrDefaultAsync(c => c.UserId == userId && c.ProfessionalId == professionalId);
        }

        public async Task AddAsync(Conversation conversation)
        {
            await _context.Conversas.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Conversation conversation)
        {
            _context.Conversas.Update(conversation);
            await _context.SaveChangesAsync();
        }
    }
}
