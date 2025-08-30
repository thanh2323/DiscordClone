using System.Security.Claims;
using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using DiscordClone.Services.UserServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserQueryService _userQueryService;
        private readonly IUserStatusService _userStatusService;

        public UserController(
            IUserManagementService userManagementService,
            IUserQueryService userQueryService,
            IUserStatusService userStatusService)
        {
            _userManagementService = userManagementService;
            _userQueryService = userQueryService;
            _userStatusService = userStatusService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var result = await _userManagementService.CreateUserAsync(createUserDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userManagementService.LoginAsync(loginDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   throw new UnauthorizedAccessException("User ID not found in token");
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(string userId)
        {
            var result = await _userManagementService.GetUserByIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
        {
            var result = await _userManagementService.GetUserByEmailAsync(email);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("online")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetOnlineUsers()
        {
            var result = await _userQueryService.GetOnlineUsersAsync();
            return Ok(result);
        }
        [HttpGet("server/{serverId:int}/members")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetServerMembers(int serverId)
        {
            var result = await _userQueryService.GetUsersByServerIdAsync(serverId);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = GetCurrentUserId();
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("online-status")]
        public async Task<ActionResult<ApiResponse<bool>>> SetOnlineStatus([FromBody] bool isOnline)
        {
            var userId = GetCurrentUserId();
            var result = await _userStatusService.SetOnlineStatusAsync(userId, isOnline);
            return Ok(result);
        }

        [HttpPut("last-seen")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateLastSeen()
        {
            var userId = GetCurrentUserId();
            var result = await _userStatusService.UpdateLastSeenAsync(userId);
            return Ok(result);
        }
    }
}
