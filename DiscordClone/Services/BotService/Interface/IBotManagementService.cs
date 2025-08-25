using DiscordClone.DTOs;

namespace DiscordClone.Services.BotService.Interface
{
    public interface IBotManagementService
    {
        Task<BotDto> CreateAsync(CreateBotDto createBotDto);
        Task<BotDto?> UpdateAsync(int id, UpdateBotDto updateBotDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddToRoomAsync(int botId, int roomId);
        Task<bool> RemoveFromRoomAsync(int botId, int roomId);
        Task<string> RegenerateTokenAsync(int botId);
    }
}
