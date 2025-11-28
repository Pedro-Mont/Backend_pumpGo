using Microsoft.AspNetCore.Mvc;
using pump_go.pump_go.Bussiness.Interfaces.IServices;

namespace pump_go.pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExercises([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _exerciseService.GetAllExerciseAsync(pageNumber, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar exercícios: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(Guid id)
        {
            try
            {
                var exercise = await _exerciseService.GetExerciseByIdAsync(id);
                return Ok(exercise);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("por-grupo/{grupoMuscularId}")]
        public async Task<IActionResult> GetExerciciosPorGrupo(Guid muscleGroupId)
        {
            try
            {
                var exercises = await _exerciseService.GetExerciseByMuscleGroupAsync(muscleGroupId);
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar exercícios por grupo: {ex.Message}" });
            }
        }

        [HttpGet("grupos-musculares")]
        public async Task<IActionResult> GetAllMuscleGroup()
        {
            try
            {
                var groups = await _exerciseService.GetAllMuscleGroupAsync();
                return Ok(groups);
            }   
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar grupos musculares: {ex.Message}" });
            }
        }
    }
}
