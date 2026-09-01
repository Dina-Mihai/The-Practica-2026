using ChatApp.Application.Interfaces;
using ChatApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsParticipantAsync(int conversationId, int userId)
        {
            return await _context.ConversationParticipant
                .AnyAsync(cp => cp.ConversationID == conversationId && cp.UserID == userId);
        }
    }
}