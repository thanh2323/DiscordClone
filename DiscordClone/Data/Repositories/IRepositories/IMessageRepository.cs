using System.Linq.Expressions;
using DiscordClone.Models.DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetRoomMessagesAsync(int roomId, int pageSize = 50, int skip = 0);
        Task<IEnumerable<Message>> GetUserMessagesAsync(string userId, int pageSize = 50, int skip = 0);
        Task<IEnumerable<Message>> SearchMessagesAsync(int roomId, string searchTerm);
        Task<Message?> GetWithRepliesAsync(int messageId);
        Task<int> CountAsync(Expression<Func<Message, bool>> predicate);
        Task<Message?> GetMessageWithRoomAndUserAsync(int messageId);
        Task<Message> AddAsync(Message message);
        Task<bool> UpdateAsync(Message message);
        Task<bool> DeleteAsync(Message messageId); // hard delete
        Task SoftDeleteAsync(Message messageId);   // soft delete
    }
}
