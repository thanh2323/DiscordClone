using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room> AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;

        }

        public async Task<bool> DeleteAsync(Room? room)
        {
            if (room == null) return false;
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0;
            
        }
        public async Task<bool> UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Room>> GetActiveRoomsByTypeAsync(RoomType type)
        {
           return await _context.Rooms
                .Where(r => r.Type == type && r.IsActive)
                .Include(r => r.Channel)
                .ThenInclude(c => c.Server)
                .ToListAsync();
        }

        public async Task<int> GetActiveUsersCountAsync(int roomId)
        {
           return await _context.RoomMembers
                .Where(rm => rm.RoomId == roomId && rm.IsActive)
                .CountAsync();
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
           return await _context.Rooms.ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int roomId)
        {

            return await _context.Rooms.FindAsync(roomId);
        }

        public async Task<IEnumerable<Room>> GetChannelRoomsAsync(int channelId)
        {
            return await _context.Rooms
                .Where(r => r.ChannelId == channelId && r.IsActive)
                //.OrderBy(r => r.Position)
                .ToListAsync();
        }

        public async Task<Room?> GetWithMessagesAsync(int roomId, int pageSize = 50, int skip = 0)
        {
            return await _context.Rooms
                .Include(r => r.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(pageSize))
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(r => r.Id == roomId);

        }

      
    }
}
