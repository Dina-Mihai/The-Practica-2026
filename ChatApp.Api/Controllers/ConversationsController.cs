using ChatApp.Application.DTOs.PublicMassage;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Application.DTOs.PublicConversation;

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

        [HttpGet]
        public async Task<IActionResult> GetMyConversations()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var conversations = await _conversationRepository.GetUserConversationsAsync(userId);
            var result = conversations.Select(ConversationMaper.MapToDTO);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation(CreateConversationDto request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            if (request.ParticipantIds == null || request.ParticipantIds.Count == 0)
            {
                return BadRequest("Trebuie specificat cel putin un participant.");
            }

            var conversation = await _conversationRepository.CreateConversationAsync(userId, request.ParticipantIds, request.ConversationName);

            return Ok(new { conversationId = conversation.ID });
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