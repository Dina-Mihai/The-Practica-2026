using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface IConversationRepository
    {
        Task<bool> IsParticipantAsync(int conversationId, int userId);
        Task<Conversation> CreateConversationAsync(int creatorUserId, List<int> participantIds, string? conversationName);
        Task<List<Conversation>> GetUserConversationsAsync(int userId);
    }
}
