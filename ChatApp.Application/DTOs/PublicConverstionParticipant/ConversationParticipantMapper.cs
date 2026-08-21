using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicConverstionParticipant
{
    public static class ConversationParticipantMapper
    {
        public static ConversationParticipantDTO MapToDTO_ConversationParticioant(ConversationParticipant conversation_participant)
        {
            return new ConversationParticipantDTO
            {
                ConversationID=conversation_participant.ConversationID
            };
        }
    }
}
