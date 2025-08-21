using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.MessageServices.Interface
{
    public interface IMessageQueryService
    {
        Task<ApiResponse<MessageDto>> GetMessageByIdAsync(int messageId, string userId);
        Task<ApiResponse<PagedResult<MessageDto>>> GetRoomMessagesAsync(int roomId, string userId, PaginationParams pagination);
        Task<ApiResponse<IEnumerable<MessageDto>>> SearchMessagesAsync(MessageSearchDto searchDto, string userId);
    }
}
