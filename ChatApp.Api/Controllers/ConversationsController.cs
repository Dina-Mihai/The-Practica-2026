using ChatApp.Application.DTOs.PublicMassage;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/conversations")]
    public class ConversationsController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;

        public ConversationsController(
            IMessageRepository messageRepository,
            IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(int conversationId, int page = 1, int pageSize = 20)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            bool isParticipant = await _conversationRepository.IsParticipantAsync(conversationId, userId);
            if (!isParticipant)
            {
                return Forbid();
            }

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var messages = await _messageRepository.GetMessagesAsync(conversationId, page, pageSize);

            var result = messages.Select(MassageMapper.MapToDTO_Message);

            return Ok(result);
        }
    }
}