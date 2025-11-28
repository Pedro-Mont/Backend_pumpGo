using Microsoft.EntityFrameworkCore;
using pump_go.Data.Context;
using pump_go.Entities;
using pump_go.Entities.Enums;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Data.Helpers;

namespace pump_go.pump_go.Data.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> GetByIdAsync(Guid id)
        {
            return await _context.Exercicios.FindAsync(id);
        }

        public async Task<PagedList<Exercise>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Exercicios.Include(e => e.MuscleGroup).OrderBy(e => e.Name);
            return await PagedList<Exercise>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(Guid muscleGroupId)
        {
            return await _context.Exercicios.Where(e => e.MuscleGroupId == muscleGroupId).OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByPlaceTypeAsync(PlaceType placeType)
        {
            return await _context.Exercicios.Where(e => e.PlaceType == placeType).OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> SearchByNameAsync(string name)
        {
            return await _context.Exercicios.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercicios.AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _context.Exercicios.Update(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var exercise = await _context.Exercicios.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercicios.Remove(exercise);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Exercise>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Exercicios.Where(ex => ids.Contains(ex.Id)).ToListAsync();
        }
    }
}
