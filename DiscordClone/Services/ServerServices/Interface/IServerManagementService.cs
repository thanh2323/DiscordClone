using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.ServerServices.Interface
{
    public interface IServerManagementService
    {
        Task<ApiResponse<ServerDto>> CreateServerAsync(CreateServerDto createServerDto, string adminId);
        Task<ApiResponse<ServerDto>> UpdateServerAsync(int serverId, UpdateServerDto updateServerDto, string userId);
        Task<ApiResponse<bool>> DeleteServerAsync(int serverId, string userId);
        Task<ApiResponse<ServerDto>> GetServerByIdAsync(int serverId, string userId);   
       
    }
}
