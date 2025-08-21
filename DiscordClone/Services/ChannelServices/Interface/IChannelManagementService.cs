using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using System.Threading.Tasks;

namespace DiscordClone.Services.ChannelService.Interface
{
    public interface IChannelManagementService
    {
        Task<ApiResponse<ChannelDto>> CreateAsync(CreateChannelDto createChannelDto, string userId);
        Task<ApiResponse<ChannelDto>> UpdateAsync(int channelId, UpdateChannelDto updateChannelDto, string userId);
        Task<ApiResponse<bool>> DeleteAsync(int channelId, string userId);

        Task<ApiResponse<bool>> ReorderAsync(int serverId, ReorderChannelsDto reorderDto, string userId);
    }
}

