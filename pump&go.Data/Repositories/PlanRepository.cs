using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.pump_go.Bussiness.Interfaces.IRepositories;

namespace pump_go.pump_go.Data.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public PlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Plan> GetByIdAsync(int id)
        {
            return await _context.Planos.FindAsync(id);
        }

        public async Task<Plan> GetByNameAsync(string name)
        {
            return await _context.Planos.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await _context.Planos.ToListAsync();
        }

        public async Task AddAsync(Plan plan)
        {
            await _context.Planos.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Plan plan)
        {
            _context.Planos.Update(plan);
            await _context.SaveChangesAsync();
        }
    }
}
