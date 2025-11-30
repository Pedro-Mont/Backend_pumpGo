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
            var rotinaExists = _context.ChangeTracker.Entries<Routine>()
                .FirstOrDefault(e => e.Entity.Id == routine.Id)?.Entity;

            if (rotinaExists == null)
            {
                _context.RotinasDeTreino.Attach(routine);
                _context.Entry(routine).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(rotinaExists).CurrentValues.SetValues(routine);
            }

            var itensNoBanco = await _context.ItensRotina
                .Where(i => i.RoutineId == routine.Id)
                .ToListAsync();

            _context.ItensRotina.RemoveRange(itensNoBanco);

            foreach (var newItem in routine.RoutineItems)
            {
                newItem.RoutineId = routine.Id;
                _context.Entry(newItem).State = EntityState.Added;
            }

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
