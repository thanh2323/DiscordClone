using DiscordClone.DTOs.Common;

namespace DiscordClone.Services.UserServices.Interface
{
    public interface IUserStatusService
    {
        Task<ApiResponse<bool>> SetOnlineStatusAsync(string userId, bool isOnline);
        Task<ApiResponse<bool>> UpdateLastSeenAsync(string userId);
    }
}
