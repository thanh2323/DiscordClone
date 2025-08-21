using DiscordClone.Data.Repositories;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.ServerServices.Interface;

namespace DiscordClone.Services.ServerServices
{
    public class ServerInviteService : IServerInviteService
    {

        private readonly IServerRepository _serverRepository;
        public ServerInviteService(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }
        public async Task<ApiResponse<string>> GenerateInviteCodeAsync(int serverId, string userId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<string>.ErrorResult("Access denied. Only admins can generate invite codes.");
                }

                var server = await _serverRepository.GetByIdAsync(serverId);
                if (server == null)
                {
                    return ApiResponse<string>.ErrorResult("Server not found");
                }

                server.InviteCode = await _serverRepository.GenerateUniqueInviteCodeAsync();
                await _serverRepository.UpdateAsync(server);

                return ApiResponse<string>.SuccessResult(server.InviteCode, "Invite code generated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult("Error generating invite code", ex.Message);
            }
        }
    }
}