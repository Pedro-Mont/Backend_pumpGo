using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pump_go.Entities;
using pump_go.pump_go.Bussiness.DTOs.Routine;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using System.Security.Claims;

namespace pump_go.pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class RoutineController : ControllerBase
    {
        private readonly IRoutineService _routineService;

        public RoutineController(IRoutineService routineService)
        {
            _routineService = routineService;
        }

        private Guid GetUserIdDoToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("Token inválido ou ID do usuário não encontrado.");
            }
            return Guid.Parse(userIdString);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoutine([FromBody] RoutineCreateDTO newRoutineDto)
        {
            try
            {
                var userId = GetUserIdDoToken();
                var routine = await _routineService.CreateNewRoutine(userId, newRoutineDto);
                return CreatedAtAction(nameof(GetRoutineById), new { id = routine.Id }, routine);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("minhas")]
        public async Task<IActionResult> GetMyRoutines()
        {
            try
            {
                var userId = GetUserIdDoToken();
                var routines = await _routineService.GetUserRoutineAsync(userId);
                return Ok(routines);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("forum")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicRoutines()
        {
            var routines = await _routineService.GetPublicRoutinesAsync();
            return Ok(routines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoutineById(Guid id)
        {
            try
            {
                var routine = await _routineService.GetDetailedRoutineAsync(id);
                return Ok(routine);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoutine(Guid id)
        {
            try
            {
                var usuarioId = GetUserIdDoToken();
                await _routineService.DeleteRoutineAsync(id, usuarioId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoutine(Guid id, [FromBody] RoutineUpdateDTO routintDto)
        {
            try
            {
                var usuarioId = GetUserIdDoToken();
                await _routineService.UpdateRoutineAsync(id, usuarioId, routintDto);
                return NoContent(); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
