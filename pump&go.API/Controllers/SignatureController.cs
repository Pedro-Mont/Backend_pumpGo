using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using pump_go.pump_go.Bussiness.DTOs.Signature;

namespace pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SignatureController : ControllerBase
    {
        private readonly ISignatureService _signatureService;

        public SignatureController(ISignatureService signatureService)
        {
            _signatureService = signatureService;
        }
        private Guid GetUserIdDoToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("ID do usuário não encontrado no token.");
            }
            return Guid.Parse(userIdString);
        }

        [HttpGet("minha-assinatura")]
        public async Task<IActionResult> GetMySignature()
        {
            var userId = GetUserIdDoToken();
            var signature = await _signatureService.GetUsersSignatureAsync(userId);
            return Ok(signature);
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradePlano([FromBody] UpgradePlanDTO dto)
        {
            var userId = GetUserIdDoToken();
            var updatedSignature = await _signatureService.UpgradeSignatureAsync(userId, dto.PlanName);
            return Ok(updatedSignature);
        }
    }
}