using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;

namespace pump_go.pump_go.Data.Repositories
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly ApplicationDbContext _context;

        public SignatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Signature> GetByIdAsync(Guid id)
        {
            return await _context.Assinaturas.FindAsync(id);
        }

        public async Task<Signature> GetByUserIdAsync(Guid userId)
        {
            return await _context.Assinaturas.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task AddAsync(Signature signature)
        {
            await _context.Assinaturas.AddAsync(signature);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Signature signature)
        {
            _context.Assinaturas.Update(signature);
            await _context.SaveChangesAsync();
        }
    }
}
