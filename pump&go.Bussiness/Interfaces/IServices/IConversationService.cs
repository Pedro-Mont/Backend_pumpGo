using pump_go.pump_go.Bussiness.DTOs.Conversation;

namespace pump_go.pump_go.Bussiness.Interfaces.IServices
{
    public interface IConversationService
    {
        Task<ConversationDTO> StartConversationAsync(Guid userId, Guid professionalId);
        Task<MessageDTO> SendMessageAsync(NewMessageDTO newMessageDTO);
        Task<IEnumerable<ConversationResumeDTO>> GetUserConversationsAsync(Guid userId);
        Task<IEnumerable<MessageDTO>> GetConversationMessagesAsync(Guid conversationId, Guid userId);
    }
}
