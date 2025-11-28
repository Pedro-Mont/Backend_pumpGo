using pump_go.pump_go.Bussiness.DTOs.Routine;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface IRoutineService
    {
        Task<DetailedRoutineDTO> CreateNewRoutine(Guid userId, RoutineCreateDTO newRoutine);
        Task<DetailedRoutineDTO> GetDetailedRoutineAsync(Guid routineId);
        Task<IEnumerable<RoutineResumeDTO>> GetUserRoutineAsync(Guid UserId);
        Task<IEnumerable<RoutineResumeDTO>> GetPublicRoutinesAsync();
        Task<bool> UpdateRoutineAsync(Guid routineId, Guid userId, RoutineUpdateDTO routineDTO);
        Task<bool> DeleteRoutineAsync(Guid routineId, Guid userId);
    }
}
