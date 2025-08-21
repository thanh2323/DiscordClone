using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.RoomServices.Interface
{
    public interface IRoomManagementService
    {
        Task<ApiResponse<RoomDto>> CreateRoomAsync(CreateRoomDto createRoomDto, string userId);
        Task<ApiResponse<RoomDto>> UpdateRoomAsync(int roomId, UpdateRoomDto updateRoomDto, string userId);
        Task<ApiResponse<bool>> DeleteRoomAsync(int roomId, string userId);
    }
}
