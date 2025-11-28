using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;

namespace pump_go.pump_go.Data.Repositories
{
    public class MuscleGroupRepository : IMuscleGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public MuscleGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MuscleGroup> GetByIdAsync(Guid id)
        {
            return await _context.GruposMusculares.FindAsync(id);
        }

        public async Task<IEnumerable<MuscleGroup>> GetAllAsync()
        {
            return await _context.GruposMusculares.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task AddAsync(MuscleGroup muscleGroup)
        {
            await _context.GruposMusculares.AddAsync(muscleGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MuscleGroup muscleGroup)
        {
            _context.GruposMusculares.Update(muscleGroup);
            await _context.SaveChangesAsync();
        }
    }
}
