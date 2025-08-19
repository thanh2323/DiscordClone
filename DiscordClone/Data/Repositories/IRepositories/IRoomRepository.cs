using DiscordClone.Models;

namespace DiscordClone.Data.Repositories.IRepositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(int roomId);
        Task<IEnumerable<Room>> GetAllAsync();

        Task<Room> AddAsync(Room room);

        Task<bool> UpdateAsync(Room room);

        Task<bool> DeleteAsync(Room? room);

        Task<IEnumerable<Room>> GetChannelRoomsAsync(int channelId);
        Task<Room?> GetWithMessagesAsync(int roomId, int pageSize = 50, int skip = 0);
        Task<IEnumerable<Room>> GetActiveRoomsByTypeAsync(RoomType type);
        Task<int> GetActiveUsersCountAsync(int roomId);

    }
}
