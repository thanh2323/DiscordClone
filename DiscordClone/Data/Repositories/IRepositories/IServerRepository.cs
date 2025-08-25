using DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IServerRepository
    {

        Task<Server?> GetByIdAsync(int id);
        Task<IEnumerable<Server>> GetAllAsync();
        Task AddAsync(Server server);
        Task UpdateAsync(Server server);
        Task DeleteAsync(Server server);
        Task AddMemberAsync(ServerMember serverMember); // Add Member 
        Task<bool> RemoveMemberAsync(int serverId, string userId); // Remove Member
        Task<IEnumerable<Server>> GetUserServersAsync(string userId);
        Task<Server?> GetByInviteCodeAsync(string inviteCode);
        Task<Server?> GetWithChannelsAsync(int serverId);
        Task<Server?> GetWithMembersAsync(int serverId);
      //  Task<Server?> GetWithChannelsAndMembersAsync(int serverId);
        Task<bool> IsUserMemberAsync(int serverId, string userId);
        Task<bool> IsUserAdminAsync(int serverId, string userId);
        Task<string> GenerateUniqueInviteCodeAsync();
    }
}
