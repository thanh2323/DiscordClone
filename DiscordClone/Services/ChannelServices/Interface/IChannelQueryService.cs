using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.ChannelService.Interface
{
    public interface IChannelQueryService
    {
        Task<ApiResponse<ChannelDto>> GetChannelByIdAsync(int channelId, string userId);
        Task<ApiResponse<IEnumerable<ChannelDto>>> GetServerChannelsAsync(int serverId, string userId);
    }
}
