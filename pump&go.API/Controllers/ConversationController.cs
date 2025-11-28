using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pump_go.pump_go.Bussiness.DTOs.Conversation;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using System.Security.Claims;

namespace pump_go.pump_go.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
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

        [HttpGet("minhas-conversas")]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = GetUserIdDoToken();
            var conversations = await _conversationService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }

        [HttpPost("iniciar/{professionalId}")]
        public async Task<IActionResult> StartConversation(Guid professionalId)
        {
            var userId = GetUserIdDoToken();
            var conversationDto = await _conversationService.StartConversationAsync(userId, professionalId);
            return Ok(conversationDto);
        }

        [HttpGet("{conversationId}/mensagens")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userId = GetUserIdDoToken();
            var messages = await _conversationService.GetConversationMessagesAsync(conversationId, userId);
            return Ok(messages);
        }

        [HttpPost("enviar-mensagem")]
        public async Task<IActionResult> SendMessage([FromBody] NewMessageDTO newMessageDTO)
        {
            newMessageDTO.SenderId = GetUserIdDoToken();

            var mensagemDto = await _conversationService.SendMessageAsync(newMessageDTO);
            return Ok(mensagemDto);
        }
    }    
}
