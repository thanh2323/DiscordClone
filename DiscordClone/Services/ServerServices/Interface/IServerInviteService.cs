using DiscordClone.DTOs.Common;

namespace DiscordClone.Services.ServerServices.Interface
{
    public interface IServerInviteService
    {
        Task<ApiResponse<string>> GenerateInviteCodeAsync(int serverId, string userId);
    }
}
