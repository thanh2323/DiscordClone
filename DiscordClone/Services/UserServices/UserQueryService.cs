using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.UserServices.Interface;

namespace DiscordClone.Services.UserServices
{
    public class UserQueryService : IUserQueryService 
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserQueryService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

    


        public async Task<ApiResponse<IEnumerable<UserDto>>> GetOnlineUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetOnlineUsersAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos, "Online users retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResult("An error occurred while retrieving online users", ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetUsersByServerIdAsync(int serverId)
        {
            try
            {
                var users = await _userRepository.GetServerMembersAsync(serverId);
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos, "Server members retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResult("An error occurred while retrieving server members", ex.Message);
            }
        }
      
        
       

     


    }
}