using pump_go.Entities;
using pump_go.Entities.Enums;
using pump_go.pump_go.Data.Helpers;

namespace pump_go.Interfaces.IRepositories
{
    public interface IExerciseRepository
    {   
        Task<Exercise> GetByIdAsync(Guid id);
        Task<PagedList<Exercise>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(Guid muscleGorup);
        Task<IEnumerable<Exercise>> GetByPlaceTypeAsync(PlaceType tipoLocal);
        Task<IEnumerable<Exercise>> SearchByNameAsync(string name);
        Task AddAsync (Exercise exercise);
        Task UpdateAsync (Exercise exercise);
        Task DeleteAsync (Guid id);
        Task<IEnumerable<Exercise>> GetByIdsAsync(List<Guid> ids);
    }
}
