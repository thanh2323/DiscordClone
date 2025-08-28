using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using System.Security.Claims;
using DiscordClone.Services.RoomServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomManagementService _roomManagementService;
        private readonly IRoomQueryService _roomQueryService;

        public RoomController(
            IRoomManagementService roomManagementService,
            IRoomQueryService roomQueryService)
        {
            _roomManagementService = roomManagementService;
            _roomQueryService = roomQueryService;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet("{roomId:int}")]
        public async Task<ActionResult<ApiResponse<RoomDto>>> GetRoomById(int roomId)
        {
            var userId = GetCurrentUserId();
            var result = await _roomQueryService.GetRoomByIdAsync(roomId, userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("channel/{channelId:int}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoomDto>>>> GetChannelRooms(int channelId)
        {
            var userId = GetCurrentUserId();
            var result = await _roomQueryService.GetChannelRoomsAsync(channelId, userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoomDto>>> CreateRoom([FromBody] CreateRoomDto createRoomDto)
        {
            var userId = GetCurrentUserId();
            var result = await _roomManagementService.CreateRoomAsync(createRoomDto, userId);
            return result.Success ? CreatedAtAction(nameof(GetRoomById), new { roomId = result.Data?.Id }, result) : BadRequest(result);
        }
        [HttpPut("{roomId:int}")]
        public async Task<ActionResult<ApiResponse<RoomDto>>> UpdateRoom(int roomId, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var userId = GetCurrentUserId();
            var result = await _roomManagementService.UpdateRoomAsync(roomId, updateRoomDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{roomId:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRoom(int roomId)
        {
            var userId = GetCurrentUserId();
            var result = await _roomManagementService.DeleteRoomAsync(roomId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{roomId:int}/messages")]
        public async Task<ActionResult<ApiResponse<RoomDto>>> GetRoomWithMessages(
            int roomId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var userId = GetCurrentUserId();
            var pagination = new PaginationParams { Page = page, PageSize = pageSize };
            var result = await _roomQueryService.GetRoomWithMessagesAsync(roomId, userId, pagination);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
