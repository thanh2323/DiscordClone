using DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IChannelRepository
    {
        Task<Channel?> GetByIdAsync(int id);
        Task<IEnumerable<Channel>> GetServerChannelsAsync(int serverId);
        Task<Channel?> GetWithRoomsAsync(int channelId);
        Task<Channel> AddAsync(Channel channel);
        Task UpdateAsync(Channel channel);
        Task UpdateRangeAsync(IEnumerable<Channel> channels);
    }
}
