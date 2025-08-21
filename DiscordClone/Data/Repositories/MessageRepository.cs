using System.Linq.Expressions;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models.DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetByIdAsync(int messageId)
        {
           var message = await _context.Messages.FindAsync(messageId);
            return message;
        }
        public async Task<Message> AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteAsync(Message messageId)
        {
            if (messageId == null) return false;
            _context.Messages.Remove(messageId);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(Message message)
        {
            if (message == null) return false;
            _context.Messages.Update(message);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CountAsync(Expression<Func<Message, bool>> predicate)
        {
            return await _context.Messages.CountAsync(predicate);
        }

        public async Task<IEnumerable<Message>> GetRoomMessagesAsync(int roomId, int pageSize = 50, int skip = 0)
        {
           return await _context.Messages
                .Where(m => m.RoomId == roomId && !m.IsDeleted)
                .Include(m => m.User)
                .Include(m => m.ReplyToMessage)
                .ThenInclude(rm => rm!.User)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUserMessagesAsync(string userId, int pageSize = 50, int skip = 0)
        {
           return await _context.Messages
                .Where(m => m.UserId == userId && !m.IsDeleted)
                .Include(m => m.Room)
                .ThenInclude(r => r.Channel)
                .ThenInclude(c => c.Server)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<Message?> GetWithRepliesAsync(int messageId)
        {
           return await _context.Messages
                .Include(m => m.User)
                .Include(m => m.Replies)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<Message>> SearchMessagesAsync(int roomId, string searchTerm)
        {
            return await _context.Messages
                .Where(m => m.RoomId == roomId && !m.IsDeleted && m.Content.Contains(searchTerm))
                .Include(m => m.User)
                .OrderByDescending(m => m.CreatedAt)
                .Take(100) // Limit to 100 results for performance
                .ToListAsync();

        }


        public async Task SoftDeleteAsync(Message messageId)
        {  
            if (messageId == null) return;
            messageId.IsDeleted = true;
            messageId.Content = "[Message deleted]";
            _context.Messages.Update(messageId);
            await _context.SaveChangesAsync();
        }

        public async Task<Message?> GetMessageWithRoomAndUserAsync(int messageId)
        {
            return await _context.Messages
                .Include(m => m.Room)
                    .ThenInclude(r => r.Channel)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }
    }
}
