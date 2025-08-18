using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
     

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(User? user)
        {
            if (user == null) return false;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }


        public async Task<IEnumerable<User>> GetOnlineUsersAsync()
        {
            return await _context.Users.Where(u => u.IsOnline).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetServerMembersAsync(int serverId)
        {
            return await _context.Users
                        .Include(u => u.ServerMembersShips)
                        .Where(u => u.ServerMembersShips.Any(sm => sm.ServerId == serverId))
                        .ToListAsync();
        }

        public async Task SetOnlineStatusAsync(string userId, bool isOnline)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsOnline = isOnline;
                if (!isOnline)
                {
                    user.LastOnline = DateTime.UtcNow;
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found", nameof(userId));
            }
        }
        public async Task UpdateLastSeenAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastOnline = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found", nameof(userId));
            }
        }

        
    }
}
