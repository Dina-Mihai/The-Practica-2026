using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;

        public ChatHub(IMessageRepository messageRepository, IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
        }

        private int GetUserId()
        {
            var userIdClaim = Context.User?.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new HubException("Utilizator neautentificat corect.");
            }
            return userId;
        }

        public async Task JoinConversation(string conversationId)
        {
            var userId = GetUserId();

            if (!int.TryParse(conversationId, out int convId))
            {
                throw new HubException("ID de conversatie invalid.");
            }

            bool isParticipant = await _conversationRepository.IsParticipantAsync(convId, userId);
            if (!isParticipant)
            {
                throw new HubException("Nu ai acces la aceasta conversatie.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task SendMessageToConversation(string conversationId, string user, string message)
        {
            var userId = GetUserId();

            if (!int.TryParse(conversationId, out int convId))
            {
                throw new HubException("ID de conversatie invalid.");
            }

            bool isParticipant = await _conversationRepository.IsParticipantAsync(convId, userId);
            if (!isParticipant)
            {
                throw new HubException("Nu ai acces la aceasta conversatie.");
            }

            var newMessage = new Message
            {
                ConversationID = convId,
                SenderID = userId,
                Content = message,
                SentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                SentTime = TimeOnly.FromDateTime(DateTime.UtcNow)
            };

            await _messageRepository.AddAsync(newMessage);

            await Clients.Group(conversationId).SendAsync("ReceiveMessage", user, message);
        }
    }
}