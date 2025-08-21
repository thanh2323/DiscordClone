using DiscordClone.DTOs.Common;
using DiscordClone.DTOs;

namespace DiscordClone.Services.MessageServices.Interface
{
    public interface IMessageManagementService
    {
        Task<ApiResponse<MessageDto>> SendMessageAsync(CreateMessageDto createMessageDto, string userId);
        Task<ApiResponse<MessageDto>> UpdateMessageAsync(int messageId, UpdateMessageDto updateMessageDto, string userId);
        Task<ApiResponse<bool>> DeleteMessageAsync(int messageId, string userId);
    }
}
