using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs.Common;
using DiscordClone.Services.UserServices.Interface;

namespace DiscordClone.Services.UserServices
{
    public class UserStatusService : IUserStatusService
    {
        private readonly IUserRepository _userRepository;
    
        public UserStatusService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
           
        }

        public async Task<ApiResponse<bool>> SetOnlineStatusAsync(string userId, bool isOnline)
        {
            try
            {
                await _userRepository.SetOnlineStatusAsync(userId, isOnline);
                return ApiResponse<bool>.SuccessResult(true, isOnline ? "User is now online" : "User is now offline");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error updating online status", ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateLastSeenAsync(string userId)
        {
            try
            {
                await _userRepository.UpdateLastSeenAsync(userId);
                return ApiResponse<bool>.SuccessResult(true, "Last seen updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error updating last seen", ex.Message);
            }
        }
    }
}
