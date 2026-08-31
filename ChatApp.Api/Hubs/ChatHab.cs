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

        public ChatHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(int conversationId, string user, string message)
        {
            var userIdClaim = Context.User?.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int senderId))
            {
                throw new HubException("Utilizator neautentificat corect.");
            }

            var newMessage = new Message
            {
                ConversationID = conversationId,
                SenderID = senderId,
                Content = message,
                SentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                SentTime = TimeOnly.FromDateTime(DateTime.UtcNow)
            };

            await _messageRepository.AddAsync(newMessage);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}