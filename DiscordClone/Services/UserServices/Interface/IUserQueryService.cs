using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;

namespace DiscordClone.Services.UserServices.Interface
{
    public interface IUserQueryService
    {
        Task<ApiResponse<IEnumerable<UserDto>>> GetOnlineUsersAsync();
        Task<ApiResponse<IEnumerable<UserDto>>> GetUsersByServerIdAsync(int serverId);

    }
}
