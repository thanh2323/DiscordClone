using System.Security.Claims;
using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using DiscordClone.Services.ChannelService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChannelController : Controller
    {
        private readonly IChannelManagementService _channelManagementService;
        private readonly IChannelQueryService _channelQueryService;

        public ChannelController(
            IChannelManagementService channelManagementService,
            IChannelQueryService channelQueryService)
        {
            _channelManagementService = channelManagementService;
            _channelQueryService = channelQueryService;
        }
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet("{channelId:int}")]
        public async Task<ActionResult<ApiResponse<ChannelDto>>> GetChannelById(int channelId)
        {
            var userId = GetCurrentUserId();
            var result = await _channelQueryService.GetChannelByIdAsync(channelId, userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("server/{serverId:int}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ChannelDto>>>> GetServerChannels(int serverId)
        {
            var userId = GetCurrentUserId();
            var result = await _channelQueryService.GetServerChannelsAsync(serverId, userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ChannelDto>>> CreateChannel([FromBody] CreateChannelDto createChannelDto)
        {
            var userId = GetCurrentUserId();
            var result = await _channelManagementService.CreateAsync(createChannelDto, userId);
            return result.Success ? CreatedAtAction(nameof(GetChannelById), new { channelId = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{channelId:int}")]
        public async Task<ActionResult<ApiResponse<ChannelDto>>> UpdateChannel(int channelId, [FromBody] UpdateChannelDto updateChannelDto)
        {
            var userId = GetCurrentUserId();
            var result = await _channelManagementService.UpdateAsync(channelId, updateChannelDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("{channelId:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteChannel(int channelId)
        {
            var userId = GetCurrentUserId();
            var result = await _channelManagementService.DeleteAsync(channelId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("server/{serverId:int}/reorder")]
        public async Task<ActionResult<ApiResponse<bool>>> ReorderChannels(int serverId, [FromBody] ReorderChannelsDto reorderDto)
        {
            var userId = GetCurrentUserId();
            var result = await _channelManagementService.ReorderAsync(serverId, reorderDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
