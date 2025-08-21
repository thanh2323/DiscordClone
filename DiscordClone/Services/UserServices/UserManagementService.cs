using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.UserServices.Interface;

namespace DiscordClone.Services.UserServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManagementService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var normalizedEmail = createUserDto.Email.Trim().ToLowerInvariant();
                var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);
                if (existingUser != null)
                {
                    return ApiResponse<UserDto>.ErrorResult("Email already exists");
                }
                var user = _mapper.Map<User>(createUserDto);
                var createdUser = await _userRepository.AddAsync(user);
                var userDto = _mapper.Map<UserDto>(createdUser);
                return ApiResponse<UserDto>.SuccessResult(userDto, "User created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult("An error occurred while creating the user", ex.Message);
            }
        }


        public async Task<ApiResponse<UserDto>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return ApiResponse<UserDto>.ErrorResult("User not found");
                }
                var userDto = _mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.SuccessResult(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult("An error occurred while retrieving the user", ex.Message);
            }
        }
        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserDto>.ErrorResult("User not found");
                }
                var userDto = _mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.SuccessResult(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult("An error occurred while retrieving the user", ex.Message);
            }
        }
        public async Task<ApiResponse<UserDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserDto>.ErrorResult("User not found");
                }
                _mapper.Map(updateUserDto, user);
                var updated = await _userRepository.UpdateAsync(user);
                if (!updated)
                {
                    return ApiResponse<UserDto>.ErrorResult("Failed to update user");
                }
                var userDto = _mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.SuccessResult(userDto, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult("An error occurred while updating the user", ex.Message);
            }
        }

    }
}
