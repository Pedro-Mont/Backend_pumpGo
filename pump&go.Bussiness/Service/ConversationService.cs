using Microsoft.AspNetCore.SignalR;
using pump_go.Entities;
using pump_go.Interfaces.IRepositories;
using pump_go.pump_go.Bussiness.DTOs.Conversation;
using pump_go.pump_go.Bussiness.Exceptions;
using pump_go.pump_go.Bussiness.Interfaces.IServices;
using pump_go.pump_go.Data.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace pump_go.pump_go.Bussiness.Service
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly ISignatureService _signatureService;

        public ConversationService(IConversationRepository conversationRepository, IUserRepository userRepository, IProfessionalRepository professionalRepository, ISignatureService signatureService)
        {
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _professionalRepository = professionalRepository;
            _signatureService = signatureService;
        }

        public async Task<ConversationDTO> StartConversationAsync(Guid userId, Guid professionalId)
        {
            var professional = await _professionalRepository.GetByIdAsync(professionalId);
            if (professional == null)
            {
                throw new NotFoundException("Profissional não encontrado");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            bool hasAccess = await _signatureService.UserHasAccessToProfessional(userId, professional.Specialty);

            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("Seu plano atual não permite iniciar conversas com este tipo de profissional.");
            }

            var existingConversation = await _conversationRepository.FindByUserAndProfessionalAsync(userId, professionalId);

            if (existingConversation != null)
            {
                return new ConversationDTO
                {
                    Id = existingConversation.Id,
                    UserId = existingConversation.UserId,
                    ProfessionalId = existingConversation.ProfessionalId,
                    ProfessionalName = professional.Name,
                    Messages = existingConversation.Messages.Select(m => new MessageDTO
                    {
                        Id = m.Id,
                        SenderId = m.Sender,
                        SenderName = m.Sender == userId ? user.Name : professional.Name,
                        Content = m.Content,
                        SentDate = m.SentDate
                    }).OrderBy(m => m.SentDate).ToList()
                };
            }

            var newConversation = new Conversation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProfessionalId = professionalId,
                StartDate = DateTime.UtcNow,
                Messages = new List<Message>()
            };

            await _conversationRepository.AddAsync(newConversation);

            return new ConversationDTO
            {
                Id = newConversation.Id,
                UserId = newConversation.UserId,
                ProfessionalId = newConversation.ProfessionalId,
                ProfessionalName = professional.Name,
                Messages = new List<MessageDTO>()
            };
        }

        public async Task<MessageDTO> SendMessageAsync(NewMessageDTO newMessageDTO)
        {
            var conversation = await _conversationRepository.GetByIdAsync(newMessageDTO.ConversationId);

            if (conversation == null)
            {
                throw new NotFoundException("Conversa não encontrada.");
            }

            if (newMessageDTO.SenderId != conversation.UserId && newMessageDTO.SenderId != conversation.ProfessionalId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para enviar mensagens nesta conversa.");
            }

            var newMessage = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = newMessageDTO.ConversationId,
                Sender = newMessageDTO.SenderId,
                Content = newMessageDTO.Content,
                SentDate = DateTime.UtcNow
            };

            if (conversation.Messages == null) conversation.Messages = new List<Message>();

            conversation.Messages.Add(newMessage);

            await _conversationRepository.UpdateAsync(conversation);

            string senderName = "Desconhecido";

            if (newMessage.Sender == conversation.UserId)
            {
                senderName = conversation.User?.Name ?? "Você";
            }
            else if (newMessage.Sender == conversation.ProfessionalId)
            {
                senderName = conversation.Professional?.Name ?? "Profissional";
            }

            return new MessageDTO
            {
                Id = newMessage.Id,
                SenderId = newMessage.Sender,
                SenderName = senderName,
                Content = newMessage.Content,
                SentDate = newMessage.SentDate
            };
        }

        public async Task<IEnumerable<MessageDTO>> GetConversationMessagesAsync(Guid conversationId, Guid userId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
            {
                throw new NotFoundException("Conversa não encontrada");
            }

            if (userId != conversation.UserId && userId != conversation.ProfessionalId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar estas mensagens.");
            }

            var userName = conversation.User?.Name ?? "Usuário";
            var professionalName = conversation.Professional?.Name ?? "Profissional";

            return conversation.Messages.Select(message => new MessageDTO
            {
                Id = message.Id,
                SenderId = message.Sender,
                SenderName = message.Sender == conversation.UserId ? userName : professionalName,
                Content = message.Content,
                SentDate = message.SentDate
            }).OrderBy(m => m.SentDate);
        }

        public async Task<IEnumerable<ConversationResumeDTO>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _conversationRepository.GetByUserIdAsync(userId);

            var dtos = conversations.Select(conversation => new ConversationResumeDTO
            {
                Id = conversation.Id,
                ProfessionalName = conversation.Professional?.Name ?? "Profissional",
                LastMessage = conversation.Messages.OrderByDescending(m => m.SentDate).FirstOrDefault()?.Content ?? "Inicie a conversa"
            });

            return dtos;
        }
    }
}