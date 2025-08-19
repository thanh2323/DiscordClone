using DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IBotRepository
    {
        // CRUD Bot
        Task<Bot?> GetByIdAsync(int botId);
        Task<Bot> AddAsync(Bot bot);
        Task<bool> UpdateAsync(Bot bot);
        Task<bool> DeleteAsync(int botId);

        // Bot query
        Task<IEnumerable<Bot>> GetServerBotsAsync(int serverId);
        Task<Bot?> GetByTokenAsync(string token);
        Task<Bot?> GetWithRoomsAsync(int botId);
        Task<IEnumerable<Bot>> GetActiveBotsAsync();

        // BotRoom management
        Task<bool> IsBotInRoomAsync(int botId, int roomId);
        Task AddBotToRoomAsync(int botId, int roomId);
        Task RemoveBotFromRoomAsync(int botId, int roomId);
        Task<IEnumerable<Room>> GetBotRoomsAsync(int botId);

        // Utility
        Task<string> GenerateUniqueBotTokenAsync();
    }
}
