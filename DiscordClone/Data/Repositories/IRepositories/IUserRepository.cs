using DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> AddAsync(User user, string password);

        Task<bool> UpdateAsync(User user);

        Task<bool> DeleteAsync(User user);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetOnlineUsersAsync();

        Task<IEnumerable<User>> GetServerMembersAsync(int serverId);

        Task UpdateLastSeenAsync(string userId);

        Task SetOnlineStatusAsync(string userId, bool isOnline);

    }
}
