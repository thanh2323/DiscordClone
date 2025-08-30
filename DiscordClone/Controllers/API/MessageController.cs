using System.Security.Claims;
using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;
using DiscordClone.Services.MessageServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : Controller
    {

        private readonly IMessageManagementService _messageManagementService;
        private readonly IMessageQueryService _messageQueryService;

        public MessageController(
            IMessageManagementService messageManagementService,
            IMessageQueryService messageQueryService)
        {
            _messageManagementService = messageManagementService;
            _messageQueryService = messageQueryService;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet("{messageId:int}")]
        public async Task<ActionResult<ApiResponse<MessageDto>>> GetMessageById(int messageId)
        {
            var userId = GetCurrentUserId();
            var result = await _messageQueryService.GetMessageByIdAsync(messageId, userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("room/{roomId:int}")]
        public async Task<ActionResult<ApiResponse<PagedResult<MessageDto>>>> GetRoomMessages(
            int roomId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var userId = GetCurrentUserId();
            var pagination = new PaginationParams { Page = page, PageSize = pageSize };
            var result = await _messageQueryService.GetRoomMessagesAsync(roomId, userId, pagination);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MessageDto>>> SendMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var userId = GetCurrentUserId();
            var result = await _messageManagementService.SendMessageAsync(createMessageDto, userId);
            return result.Success ? CreatedAtAction(nameof(GetMessageById), new { messageId = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{messageId:int}")]
        public async Task<ActionResult<ApiResponse<MessageDto>>> UpdateMessage(int messageId, [FromBody] UpdateMessageDto updateMessageDto)
        {
            var userId = GetCurrentUserId();
            var result = await _messageManagementService.UpdateMessageAsync(messageId, updateMessageDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{messageId:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMessage(int messageId)
        {
            var userId = GetCurrentUserId();
            var result = await _messageManagementService.DeleteMessageAsync(messageId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MessageDto>>>> SearchMessages([FromBody] MessageSearchDto searchDto)
        {
            var userId = GetCurrentUserId();
            var result = await _messageQueryService.SearchMessagesAsync(searchDto, userId);
            return Ok(result);
        }
    }
}
