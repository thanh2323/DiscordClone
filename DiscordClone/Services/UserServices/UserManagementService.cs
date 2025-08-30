using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.UserServices.Interface;
using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Services.UserServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;


        public UserManagementService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {

            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var normalizedEmail = createUserDto.Email.Trim().ToLowerInvariant();
                var userName = createUserDto.UserName.Trim();
                var existingUserEmail = await _userRepository.GetByEmailAsync(normalizedEmail);
                var exitsingUserName = await _userRepository.GetByUsernameAsync(userName);
                if (existingUserEmail != null)
                {
                    return ApiResponse<UserDto>.ErrorResult("Email already exists");
                }
                if(exitsingUserName != null)
                {
                    return ApiResponse<UserDto>.ErrorResult("User Name already exists");
                }

                var user = _mapper.Map<User>(createUserDto);
                var createdUser = await _userRepository.AddAsync(user, createUserDto.Password);
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

        public async Task<ApiResponse<UserDto>> LoginAsync(LoginDto loginDto)
        {
            bool isEmail = loginDto.Identifier.Contains("@");

            var user = isEmail
                ? await _userRepository.GetByEmailAsync(loginDto.Identifier)
                : await _userRepository.GetByUsernameAsync(loginDto.Identifier);

            if (user == null)
                return ApiResponse<UserDto>.ErrorResult("User not found");

           /* if (string.IsNullOrEmpty(user.PasswordHash))
                return ApiResponse<UserDto>.ErrorResult("Password not set for this account");
*/

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                return ApiResponse<UserDto>.ErrorResult("Invalid password");

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.SuccessResult(userDto, "Login successful");
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
