using System.Security.Claims;
using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using DiscordClone.Services.ServerServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServerController : Controller
    {
        private readonly IServerInviteService _serverInviteService;
        private readonly IServerManagementService _serverManagementService;
        private readonly IServerMemberService _serverMemberService;

        public ServerController(
            IServerInviteService serverInviteService,
            IServerManagementService serverManagementService,
            IServerMemberService serverMemberService)
        {
            _serverInviteService = serverInviteService;
            _serverManagementService = serverManagementService;
            _serverMemberService = serverMemberService;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet("{serverId:int}")]
        public async Task<ActionResult<ApiResponse<ServerDto>>> GetServerById(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverManagementService.GetServerByIdAsync(serverId, userId);
            return result.Success ? Ok(result) : NotFound(result);
        }


        [HttpGet("user-servers")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ServerDto>>>> GetUserServers()
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.GetUserServersAsync(userId);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<ServerDto>>> CreateServer([FromBody] CreateServerDto createServerDto)
        {
            var userId = GetCurrentUserId();
            var result = await _serverManagementService.CreateServerAsync(createServerDto, userId);
            return result.Success ? CreatedAtAction(nameof(GetServerById), new { serverId = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{serverId:int}")]
        public async Task<ActionResult<ApiResponse<ServerDto>>> UpdateServer(int serverId, [FromBody] UpdateServerDto updateServerDto)
        {
            var userId = GetCurrentUserId();
            var result = await _serverManagementService.UpdateServerAsync(serverId, updateServerDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{serverId:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteServer(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverManagementService.DeleteServerAsync(serverId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("join/{inviteCode}")]
        public async Task<ActionResult<ApiResponse<ServerDto>>> JoinServerByInvite(string inviteCode)
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.JoinServerByInviteAsync(inviteCode, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{serverId:int}/leave")]
        public async Task<ActionResult<ApiResponse<bool>>> LeaveServer(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.LeaveServerAsync(serverId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{serverId:int}/generate-invite")]
        public async Task<ActionResult<ApiResponse<string>>> GenerateInviteCode(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverInviteService.GenerateInviteCodeAsync(serverId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{serverId:int}/kick/{targetUserId}")]
        public async Task<ActionResult<ApiResponse<bool>>> KickMember(int serverId, string targetUserId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.KickMemberAsync(serverId, targetUserId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{serverId:int}/membership")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckMembership(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.IsUserMemberAsync(serverId, userId);
            return Ok(result);
        }

        [HttpGet("{serverId:int}/admin-status")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckAdminStatus(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _serverMemberService.IsUserAdminAsync(serverId, userId);
            return Ok(result);
        }
    }
}
