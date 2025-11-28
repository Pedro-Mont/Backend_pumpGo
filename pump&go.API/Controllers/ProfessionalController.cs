using Microsoft.AspNetCore.Mvc;
using pump_go.Entities.Enums;
using pump_go.pump_go.Bussiness.Interfaces.IServices;

namespace pump_go.pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService _professionalService;

        public ProfessionalController(IProfessionalService professionalService)
        {
            _professionalService = professionalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableProfessionais([FromQuery] ProfessionalType specialty)
        {
            try
            {
                var professionals = await _professionalService.GetProfessionalAsync(specialty);
                return Ok(professionals);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar profissionais: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfessionalProfile(Guid id)
        {
            try
            {
                var professional = await _professionalService.GetProfessionalProfileAsync(id);
                return Ok(professional);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
