using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.RoomServices.Interface
{
    public interface IRoomQueryService
    {
        Task<ApiResponse<RoomDto>> GetRoomByIdAsync(int roomId, string userId);
        Task<ApiResponse<IEnumerable<RoomDto>>> GetChannelRoomsAsync(int channelId, string userId);
        Task<ApiResponse<RoomDto>> GetRoomWithMessagesAsync(int roomId, string userId, PaginationParams pagination);
    }
}
