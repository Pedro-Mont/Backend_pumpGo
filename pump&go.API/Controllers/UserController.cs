using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pump_go.pump_go.Bussiness.DTOs.User;
using pump_go.pump_go.Bussiness.DTOs.UserDTO;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using System.Security.Claims;

namespace pump_go.pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO registrationDto)
        {
            var userDto = await _userService.RegisterNewUserAsync(registrationDto);
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _userService.AuthenticateAsync(loginDto);
            return Ok(new { token = token });
        }

        [Authorize]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var userId = GetUserIdDoToken();
            var usuarioDto = await _userService.GetUserInfoAsync(userId);
            return Ok(usuarioDto);
        }
    }
}
