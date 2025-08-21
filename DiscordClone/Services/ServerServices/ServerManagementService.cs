using System.Diagnostics;
using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.ServerServices.Interface;

namespace DiscordClone.Services.ServerServices
{
    public class ServerManagementService : IServerManagementService
    {
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;
        public ServerManagementService(IServerRepository serverRepository, IMapper mapper)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ServerDto>> CreateServerAsync(CreateServerDto createServerDto, string adminId)
        {
            try
            {
                var server = _mapper.Map<Server>(createServerDto);
                server.AdminId = adminId;
                server.InviteCode = await _serverRepository.GenerateUniqueInviteCodeAsync();

                await _serverRepository.AddAsync(server);

                var serverMember = new ServerMember
                {
                    UserId = adminId,
                    ServerId = server.Id,
                    Role = ServerRole.Admin
                };  
                await _serverRepository.AddMemberAsync(serverMember);

                // Get server with relations    
               
                var createdServer = await _serverRepository.GetWithChannelsAsync(server.Id);
                var serverDto = _mapper.Map<ServerDto>(createdServer);

                return ApiResponse<ServerDto>.SuccessResult(serverDto, "Server created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ServerDto>.ErrorResult("Error creating server", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteServerAsync(int serverId, string userId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("You do not have permission to delete this server");
                }
                var server = await _serverRepository.GetByIdAsync(serverId);
                if (server == null)
                {
                    return ApiResponse<bool>.ErrorResult("Server not found");
                }
                await _serverRepository.DeleteAsync(server);
                return ApiResponse<bool>.SuccessResult(true, "Server deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error deleting server", ex.Message);
            }
        }

        public async Task<ApiResponse<ServerDto>> GetServerByIdAsync(int serverId, string userId)
        {
            try
            {
                var server = await _serverRepository.GetByIdAsync(serverId);
                if(server == null)
                {
                    return ApiResponse<ServerDto>.ErrorResult("Server not found");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(serverId, userId);
                if (!isMember)
                {
                    return ApiResponse<ServerDto>.ErrorResult("You are not a member of this server");
                }
                var serverWithRelations = await _serverRepository.GetWithChannelsAsync(serverId);
                var dto = _mapper.Map<ServerDto>(serverWithRelations);
                return ApiResponse<ServerDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<ServerDto>.ErrorResult("Error retrieving server", ex.Message);
            }
        }

        public async Task<ApiResponse<ServerDto>> UpdateServerAsync(int serverId, UpdateServerDto updateServerDto, string userId)
        {
            try
            {
                var server = await _serverRepository.GetByIdAsync(serverId);
                if (server == null)
                {
                    return ApiResponse<ServerDto>.ErrorResult("Server not found");
                }
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<ServerDto>.ErrorResult("You do not have permission to update this server");
                }
                _mapper.Map(updateServerDto,server);
                await _serverRepository.UpdateAsync(server);

                var updatedServer = await _serverRepository.GetWithChannelsAsync(serverId);
                var serverDto = _mapper.Map<ServerDto>(updatedServer);
                return ApiResponse<ServerDto>.SuccessResult(serverDto, "Server updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ServerDto>.ErrorResult("Error updating server", ex.Message);
            }
        }
    }
}
