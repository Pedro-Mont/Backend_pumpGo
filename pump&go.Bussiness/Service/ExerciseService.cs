using pump_go.Entities;
using pump_go.Entities.Enums;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.Exercise;
using pump_go.pump_go.Bussiness.DTOs.MuscleGroup;
using pump_go.pump_go.Bussiness.DTOs.Result;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IServices;

namespace pump_go.pump_go.Bussiness.Service
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMuscleGroupRepository _muscleGroupRepository;

        public ExerciseService(IExerciseRepository exerciseRepository, IMuscleGroupRepository muscleGroupRepository)
        {
            _exerciseRepository = exerciseRepository;
            _muscleGroupRepository = muscleGroupRepository;
        }

        public async Task<PagedResult<ExerciseDTO>> GetAllExerciseAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            var pagedList = await _exerciseRepository.GetPagedAsync(pageNumber, pageSize);

            var dtos = pagedList.Items.Select(ex => new ExerciseDTO
            {
                Id = ex.Id,
                Name = ex.Name,
                Description = ex.Description,
                PhotoURL = ex.PhotoURL,
                MuscleGroupName = ex.MuscleGroup?.Name ?? "N/A",
                Place = ex.PlaceType
            }).ToList();

            return new PagedResult<ExerciseDTO>(
                dtos,
                pagedList.TotalCount,
                pageNumber,
                pageSize
            );
        }

        public async Task<ExerciseDTO> GetExerciseByIdAsync(Guid id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            if (exercise == null)
            {
                throw new NotFoundException("Exercicio não encontrado.");
            }

            var muscleGroup = await _muscleGroupRepository.GetByIdAsync(exercise.MuscleGroupId);

            return MapToDTO(exercise, muscleGroup?.Name ?? "Não categorizado");

        }

        public async Task<IEnumerable<ExerciseDTO>> GetExerciseByMuscleGroupAsync(Guid muscleGroupId)
        {
            var muscleGroup = await _muscleGroupRepository.GetByIdAsync(muscleGroupId);
            if (muscleGroup == null)
            {
                return Enumerable.Empty<ExerciseDTO>();
            }

            var exercises = await _exerciseRepository.GetByMuscleGroupAsync(muscleGroupId);

            return exercises.Select(ex => MapToDTO(ex, muscleGroup.Name));

        }

        public async Task<IEnumerable<MuscleGroupDTO>> GetAllMuscleGroupAsync()
        {
            var grupos = await _muscleGroupRepository.GetAllAsync();

            return grupos.Select(g => new MuscleGroupDTO
            {
                Id = g.Id,
                Name = g.Name
            });
        }

        private ExerciseDTO MapToDTO(Exercise exercise, string muscleGroupName)
        {
            return new ExerciseDTO
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                PhotoURL = exercise.PhotoURL,
                MuscleGroupName = muscleGroupName,
                Place = exercise.PlaceType
            };
        }
    }
}
