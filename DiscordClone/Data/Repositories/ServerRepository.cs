using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly ApplicationDbContext _context;
        public ServerRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Server?> GetByIdAsync(int id)
        {
            return await _context.Servers.FindAsync(id);

        }

        public async Task<IEnumerable<Server>> GetAllAsync()
        {
             return await _context.Servers.ToListAsync();
        }
           

        public async Task AddAsync(Server server)
        {
            await _context.Servers.AddAsync(server);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Server server)
        {
            _context.Servers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Server server)
        {
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }


        public async Task<string> GenerateUniqueInviteCodeAsync()
        {
           string inviteCode;
            do
            {
                inviteCode = GenerateRandomCode();
            } while ( await _context.Servers.AnyAsync(s => s.InviteCode == inviteCode));
            return inviteCode;
        }

        public async Task<Server?> GetByInviteCodeAsync(string inviteCode)
        {
            return await _context.Servers
                .Include(s => s.Admin)
                .FirstOrDefaultAsync(s => s.InviteCode == inviteCode);
        }

        public async Task<IEnumerable<Server>> GetUserServersAsync(string userId)
        {
            return await _context.Servers
                .Include(s => s.Members)
                .Where(s => s.Members.Any(m => m.UserId == userId))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Server?> GetWithChannelsAsync(int serverId)
        {
            return await _context.Servers
                  .Include(s => s.Channels.OrderBy(c => c.Position))
                  .ThenInclude(c => c.Rooms)
                  .FirstOrDefaultAsync(s => s.Id == serverId);
        }

        public async Task<Server?> GetWithMembersAsync(int serverId)
        {
            return await _context.Servers
                .Include(s => s.Members)
                .ThenInclude(m => m.User)
                .Include(s => s.Admin)
                .FirstOrDefaultAsync(s => s.Id == serverId);

        }

        public async Task<bool> IsUserAdminAsync(int serverId, string userId)
        {
            var server = await _context.Servers
                .Include(s => s.Admin)
                .FirstOrDefaultAsync(s => s.Id == serverId);
            return server?.AdminId == userId;
        }

        public async Task<bool> IsUserMemberAsync(int serverId, string userId)
        {
            return await _context.ServerMembers
                .AnyAsync(sm => sm.ServerId == serverId && sm.UserId == userId);
        }

        private static string GenerateRandomCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
               .Select(s => s[random.Next(s.Length)]).ToArray());

        }
    }
}
