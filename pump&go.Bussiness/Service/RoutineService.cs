using pump_go.Entities;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.Routine;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IServices;

namespace pump_go.pump_go.Bussiness.Service
{
    public class RoutineService : IRoutineService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public RoutineService(IRoutineRepository routineRepository, IUserRepository userRepository, IExerciseRepository exerciseRepository)
        {
            _routineRepository = routineRepository;
            _userRepository = userRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<DetailedRoutineDTO> CreateNewRoutine(Guid userId, RoutineCreateDTO newRoutineDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new BadRequestException("Usuário nao encontrado, impossivel criar rotina.");
            }

            var exerciseIdsDTO = newRoutineDTO.RoutineItems.Select(item => item.ExerciseId).ToList();

            var exerciseExists = await _exerciseRepository.GetByIdsAsync(exerciseIdsDTO);

            if (exerciseExists.Count() != exerciseIdsDTO.Count)
            {
                throw new Exception("Um ou mais exercícios informados na rotina são inválidos.");
            }

            var newRoutineEntity = new Routine
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = newRoutineDTO.Name,
                Description = newRoutineDTO.Description,
                IsPublic = newRoutineDTO.IsPublic,
                RoutineItems = newRoutineDTO.RoutineItems.Select(itemDTO => new RoutineItem
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = itemDTO.ExerciseId,
                    Order = itemDTO.Order,
                    Series = itemDTO.Series,
                    RestTime = itemDTO.RestTime
                }).ToList()
            };

            await _routineRepository.AddAsync(newRoutineEntity);

            return new DetailedRoutineDTO
            {
                Id = newRoutineEntity.Id,
                Name = newRoutineEntity.Name,
                Description = newRoutineEntity.Description,
                UserId = newRoutineEntity.UserId
            };
        }

        public async Task<DetailedRoutineDTO> GetDetailedRoutineAsync(Guid routineId)
        {
            var routine = await _routineRepository.GetByIdAsync(routineId);

            if (routine == null)
            {
                throw new NotFoundException("Rotina não encontrada.");
            }

            return new DetailedRoutineDTO
            {   
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                UserId = routine.UserId,
                IsPublic = routine.IsPublic,

                RoutineItems = routine.RoutineItems.Select(item => new DetailedRoutineItemDTO
                {
                    ExerciseId = item.ExerciseId,
                    ExerciseName = item.Exercise?.Name ?? "Exercício não encontrado",
                    Order = item.Order,
                    Series = item.Series,
                    RestTime = item.RestTime
                }).OrderBy(item => item.Order).ToList()
            };
        }

        public async Task<IEnumerable<RoutineResumeDTO>> GetUserRoutineAsync(Guid userId)
        {
            var routines = await _routineRepository.GetByUserIdAsync(userId);

            return routines.Select(routine => new RoutineResumeDTO
            {
                Id = routine.Id,
                Name = routine.Name,
            });
        }

        public async Task<IEnumerable<RoutineResumeDTO>> GetPublicRoutinesAsync()
        {
            var publicRoutines = await _routineRepository.GetPublicsAsync();

            return publicRoutines.Select(routine => new RoutineResumeDTO
            {
                Id = routine.Id,
                Name = routine.Name
            });
        }

        public async Task<bool> UpdateRoutineAsync(Guid routineId, Guid userId, RoutineUpdateDTO routineDTO)
        {
            var routineToUpdate = await _routineRepository.GetByIdAsync(routineId);

            if (routineToUpdate == null)
            {
                throw new NotFoundException("Rotina não encontrada.");
            }

            if (routineToUpdate.UserId != userId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para editar esta rotina.");
            }

            var exerciseIdsDTO = routineDTO.RoutineItems.Select(item => item.ExerciseId).ToList();
            var existingExercises = await _exerciseRepository.GetByIdsAsync(exerciseIdsDTO);
            if (existingExercises.Count() != exerciseIdsDTO.Count)
            {
                throw new BadRequestException("Um ou mais exercícios informados na atualização são invalidos.");
            }

            routineToUpdate.Name = routineDTO.Name;
            routineToUpdate.Description = routineDTO.Description;
            routineToUpdate.IsPublic = routineDTO.IsPublic;
            routineToUpdate.RoutineItems.Clear();

            foreach (var itemDTO in routineDTO.RoutineItems)
            {
                routineToUpdate.RoutineItems.Add(new RoutineItem
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = itemDTO.ExerciseId,
                    Order = itemDTO.Order,
                    Series = itemDTO.Series,
                    RestTime = itemDTO.RestTime
                });
            }

            await _routineRepository.UpdateAsync(routineToUpdate);

            return true;
        }

        public async Task<bool> DeleteRoutineAsync(Guid routineId, Guid userId)
        {
            var routine = await _routineRepository.GetByIdAsync(routineId);
            if (routine == null)
            {
                throw new Exception("Rotina não encontrada.");
            }

            if (routine.UserId != userId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para deletar esta rotina.");
            }

            await _routineRepository.DeleteAsync(routineId);
            return true;
        }

        private DetailedRoutineDTO MapToDetailedDTO(Routine routine, Dictionary<Guid, string> mapExerciseNames)
        {
            return new DetailedRoutineDTO
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                UserId = routine.UserId,
                IsPublic = routine.IsPublic,
                RoutineItems = routine.RoutineItems.Select(item => new DetailedRoutineItemDTO
                {
                    ExerciseId = item.ExerciseId,
                    ExerciseName = mapExerciseNames.GetValueOrDefault(item.ExerciseId, "Exercicio não encontrado"),
                    Order = item.Order,
                    Series = item.Series,
                    RestTime = item.RestTime
                }).OrderBy(item => item.Order).ToList()
            };
        }

    }
}
