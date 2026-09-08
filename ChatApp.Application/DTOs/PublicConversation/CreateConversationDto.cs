using System.Collections.Generic;

namespace ChatApp.Application.DTOs.PublicConversation
{
    public class CreateConversationDto
    {
        public List<int> ParticipantIds { get; set; } = new();
        public string? ConversationName { get; set; }
    }
}