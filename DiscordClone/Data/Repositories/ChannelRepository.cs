using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly ApplicationDbContext _context;

        public ChannelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Channel?> GetByIdAsync(int id)
        {
            return await _context.Channels.FindAsync(id);
        }

        public async Task<Channel> AddAsync(Channel channel)
        {
             await _context.Channels.AddAsync(channel);
            await _context.SaveChangesAsync();
            return channel;
        }

        public async Task<IEnumerable<Channel>> GetServerChannelsAsync(int serverId)
        {
           return await _context.Channels
                .Where(c => c.ServerId == serverId)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }

        public Task<Channel?> GetWithRoomsAsync(int channelId)
        {
            return _context.Channels
                .Include(c => c.Rooms)
                .FirstOrDefaultAsync(c => c.Id == channelId);
        }

        public async Task UpdateAsync(Channel channel)
        {
            _context.Channels.Update(channel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Channel> channels)
        {
            _context.Channels.UpdateRange(channels);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Channel channel)
        {
            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
        }
    }
}
