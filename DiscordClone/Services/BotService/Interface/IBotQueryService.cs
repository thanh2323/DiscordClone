using DiscordClone.DTOs;

namespace DiscordClone.Services.BotService.Interface
{
    public interface IBotQueryService
    {
        Task<BotDto?> GetByIdAsync(int id);
        Task<BotDto?> GetByTokenAsync(string token);
        Task<BotDto?> GetWithRoomsAsync(int botId);
        Task<IEnumerable<BotDto>> GetServerBotsAsync(int serverId);
        Task<IEnumerable<BotDto>> GetActiveBotsAsync();
        Task<IEnumerable<RoomDto>> GetBotRoomsAsync(int botId);
        Task<bool> IsBotInRoomAsync(int botId, int roomId);
    }
}
