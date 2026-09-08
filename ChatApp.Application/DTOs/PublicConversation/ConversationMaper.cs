using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicConversation
{
    public static class ConversationMaper
    {
        public static ConversationDTO MapToDTO(Conversation conversation)
        {
            return new ConversationDTO
            {
                ConversationID = conversation.ID,
                ConversationName = conversation.ConversationName
            };
        }
    }
}
