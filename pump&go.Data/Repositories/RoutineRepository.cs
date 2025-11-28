using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;

namespace pump_go.pump_go.Data.Repositories
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly ApplicationDbContext _context;

        public RoutineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Routine> GetByIdAsync(Guid id)
        {
            return await _context.RotinasDeTreino.Include(r => r.RoutineItems).ThenInclude(item =>item.Exercise).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Routine>> GetByUserIdAsync(Guid userId)
        {
            return await _context.RotinasDeTreino.Where(r => r.UserId == userId).OrderBy(r => r.Name).ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetPublicsAsync()
        {
            return await _context.RotinasDeTreino.Where(r => r.IsPublic).OrderBy(r => r.Name).ToListAsync();
        }

        public async Task AddAsync(Routine routine)
        {
            await _context.RotinasDeTreino.AddAsync(routine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Routine routine)
        {
            _context.RotinasDeTreino.Update(routine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var routine = await _context.RotinasDeTreino.FindAsync(id);
            if (routine != null)
            {
                _context.RotinasDeTreino.Remove(routine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
