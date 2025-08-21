using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.ServerServices.Interface;

namespace DiscordClone.Services.ServerServices
{
    public class ServerMemberService : IServerMemberService
    {
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;
        public ServerMemberService(IServerRepository serverRepository, IMapper mapper)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ServerDto>>> GetUserServersAsync(string userId)
        {
            try
            {
                var server = await _serverRepository.GetUserServersAsync(userId);
                var serverDtos = _mapper.Map<IEnumerable<ServerDto>>(server);
                return ApiResponse<IEnumerable<ServerDto>>.SuccessResult(serverDtos, "User servers retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ServerDto>>.ErrorResult("Error retrieving user servers", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> IsUserAdminAsync(int serverId, string userId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                return ApiResponse<bool>.SuccessResult(isAdmin);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error checking admin status", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> IsUserMemberAsync(int serverId, string userId)
        {
            try
            {
                var isMember = await _serverRepository.IsUserMemberAsync(serverId, userId);
                return ApiResponse<bool>.SuccessResult(isMember);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error checking admin status", ex.Message);
            }
        }

        public async Task<ApiResponse<ServerDto>> JoinServerByInviteAsync(string inviteCode, string userId)
        {
            try
            {
                var server = await _serverRepository.GetByInviteCodeAsync(inviteCode);
                if (server == null)
                {
                    return ApiResponse<ServerDto>.ErrorResult("Invalid invite code");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(server.Id, userId);
                if (isMember)
                {
                    return ApiResponse<ServerDto>.ErrorResult("You are already a member of this server");
                }
                var serverMember = new ServerMember
                {
                    UserId = userId,
                    ServerId = server.Id,
                    Role = ServerRole.Member // Default role for new members
                };
                await _serverRepository.AddMemberAsync(serverMember);
                var serverDto = _mapper.Map<ServerDto>(server);
                return ApiResponse<ServerDto>.SuccessResult(serverDto, "Joined server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ServerDto>.ErrorResult("Error joining server", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> KickMemberAsync(int serverId, string targetUserId, string adminId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, adminId);
                if (!isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("Access denied. Only admins can kick members.");
                }
                var isTargetAdmin = await _serverRepository.IsUserAdminAsync(serverId, targetUserId);
                if (isTargetAdmin || targetUserId == adminId)
                {
                    return ApiResponse<bool>.ErrorResult("Cannot kick admin users");
                }
                var result = await _serverRepository.RemoveMemberAsync(serverId, targetUserId);
                if (!result)
                {
                    return ApiResponse<bool>.ErrorResult("Member not found");

                }
                return ApiResponse<bool>.SuccessResult(true, "Member kicked successfully"); 
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error kicking member", ex.Message);

            }
        }

            public async Task<ApiResponse<bool>> LeaveServerAsync(int serverId, string userId)
        {
            try
            {
               
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                if (isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("Server admin cannot leave the server. Transfer ownership or delete the server.");
                }

                var isMember = await _serverRepository.IsUserMemberAsync(serverId, userId);

                if (!isMember)
                {
                    return ApiResponse<bool>.ErrorResult("You are not a member of this server");
                }

                await _serverRepository.RemoveMemberAsync(serverId, userId);

                return ApiResponse<bool>.SuccessResult(true, "Successfully left server");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error leaving server", ex.Message);
            }
        }
    }
    }

