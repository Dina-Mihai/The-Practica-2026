using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
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
        public async Task<List<Conversation>> GetUserConversationsAsync(int userId)
        {
            var conversationIds = await _context.ConversationParticipant
                .Where(cp => cp.UserID == userId)
                .Select(cp => cp.ConversationID)
                .ToListAsync();

            return await _context.Conversation
                .Where(c => conversationIds.Contains(c.ID))
                .ToListAsync();
        }


        public async Task<Conversation> CreateConversationAsync(int creatorUserId, List<int> participantIds, string? conversationName)
        {
            var allParticipantIds = participantIds
                .Append(creatorUserId)
                .Distinct()
                .ToList();

            if (allParticipantIds.Count == 2)
            {
                var existing = await FindExistingOneToOneConversationAsync(allParticipantIds[0], allParticipantIds[1]);
                if (existing != null)
                {
                    return existing;
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var conversation = new Conversation
                {
                    ConversationName = conversationName,
                    SentTimeAndDate = DateTime.UtcNow,
                    Messages = new List<Message>(),
                    ConversationParticipants = new List<ConversationParticipant>()
                };

                await _context.Conversation.AddAsync(conversation);
                await _context.SaveChangesAsync();

                foreach (var userId in allParticipantIds)
                {
                    await _context.ConversationParticipant.AddAsync(new ConversationParticipant
                    {
                        ConversationID = conversation.ID,
                        UserID = userId
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return conversation;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<Conversation?> FindExistingOneToOneConversationAsync(int userAId, int userBId)
        {
            var candidateConversationIds = await _context.ConversationParticipant
                .Where(cp => cp.UserID == userAId)
                .Select(cp => cp.ConversationID)
                .ToListAsync();

            foreach (var convId in candidateConversationIds)
            {
                var participantIds = await _context.ConversationParticipant
                    .Where(cp => cp.ConversationID == convId)
                    .Select(cp => cp.UserID)
                    .ToListAsync();

                if (participantIds.Count == 2 && participantIds.Contains(userBId))
                {
                    return await _context.Conversation.FirstOrDefaultAsync(c => c.ID == convId);
                }
            }

            return null;
        }
    }
}