using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.UserServices.Interface
{
    public interface IUserManagementService
    {
        Task<ApiResponse<UserDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);

        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto);

        Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId);
        Task<ApiResponse<UserDto>> GetUserByEmailAsync(string email);

    }
}
