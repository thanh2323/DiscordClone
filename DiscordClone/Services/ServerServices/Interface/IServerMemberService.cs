using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.ServerServices.Interface
{
    public interface IServerMemberService
    {
        Task<ApiResponse<bool>> IsUserMemberAsync(int serverId, string userId);
        Task<ApiResponse<bool>> IsUserAdminAsync(int serverId, string userId);
        Task<ApiResponse<ServerDto>> JoinServerByInviteAsync(string inviteCode, string userId);
        Task<ApiResponse<bool>> LeaveServerAsync(int serverId, string userId);
        Task<ApiResponse<bool>> KickMemberAsync(int serverId, string targetUserId, string adminId);
        Task<ApiResponse<IEnumerable<ServerDto>>> GetUserServersAsync(string userId);
    }
}
