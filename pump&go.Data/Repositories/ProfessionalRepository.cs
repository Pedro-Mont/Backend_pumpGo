using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Entities.Enums;
using pump_go.Interfaces.IRepositories;

namespace pump_go.pump_go.Data.Repositories
{
    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfessionalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Professional> GetByIdAsync(Guid id)
        {
            return await _context.Profissionais.FindAsync(id);
        }

        public async Task<IEnumerable<Professional>> GetAllAsync()
        {
            return await _context.Profissionais.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Professional>> GetBySpecialtyAsync(ProfessionalType specialty)
        {
            return await _context.Profissionais.Where(p => p.Specialty == specialty).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task AddAsync(Professional professional)
        {
            await _context.Profissionais.AddAsync(professional);
            await _context.SaveChangesAsync();
        }
    }
}
