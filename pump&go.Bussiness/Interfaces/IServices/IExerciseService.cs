using pump_go.Entities;
using pump_go.pump_go.Bussiness.DTOs.Exercise;
using pump_go.pump_go.Bussiness.DTOs.MuscleGroup;
using pump_go.pump_go.Bussiness.DTOs.Result;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface IExerciseService
    {
        Task<ExerciseDTO> GetExerciseByIdAsync(Guid id);
        Task<PagedResult<ExerciseDTO>> GetAllExerciseAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ExerciseDTO>> GetExerciseByMuscleGroupAsync(Guid muscleGroupId);
        Task<IEnumerable<MuscleGroupDTO>> GetAllMuscleGroupAsync();
    }
}
