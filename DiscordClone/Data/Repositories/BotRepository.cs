using System.Security.Cryptography;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data.Repositories
{
    public class BotRepository : IBotRepository
    {
        private readonly ApplicationDbContext _context;

        public BotRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bot?> GetByIdAsync(int botId) =>
          await _context.Bots.FindAsync(botId);

        public async Task<Bot> AddAsync(Bot bot)
        {
            await _context.Bots.AddAsync(bot);
            await _context.SaveChangesAsync();
            return bot;
        }

        public async Task<bool> UpdateAsync(Bot bot)
        {
            _context.Bots.Update(bot);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int botId)
        {
            var bot = await _context.Bots.FindAsync(botId);
            if (bot == null) return false;

            _context.Bots.Remove(bot);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Bot>> GetServerBotsAsync(int serverId)
        {
            return await _context.Bots
                .Where(b => b.ServerId == serverId)
                .Include(b => b.BotRooms)
                .ThenInclude(br => br.Room)
                .OrderBy(b => b.Name)   
                .ToListAsync();
        }

        public async Task<Bot?> GetByTokenAsync(string token)
        {
            return await _context.Bots
                .Include(b => b.Server)
                .Include(b => b.BotRooms)
                .ThenInclude(br => br.Room)
                .FirstOrDefaultAsync(b => b.Token == token && b.IsActive);
        }

        public async Task<Bot?> GetWithRoomsAsync(int botId)
        {
            return await _context.Bots
                .Include(b => b.BotRooms)
                .ThenInclude(br => br.Room)
                .ThenInclude(r => r.Channel)
                .Include(b => b.Server)
                .FirstOrDefaultAsync(b => b.Id == botId);
        }

        public async Task<IEnumerable<Bot>> GetActiveBotsAsync()
        {
            return await _context.Bots
                .Where(b => b.IsActive)
                .Include(b => b.Server)
                .Include(b => b.BotRooms)
                .ThenInclude(br => br.Room)
                .ToListAsync();
        }

        // --- BotRoom management ---
        public async Task<bool> IsBotInRoomAsync(int botId, int roomId)
        {
            return await _context.Set<BotRoom>()
                .AnyAsync(br => br.BotId == botId && br.RoomId == roomId && br.IsActive);
        }

        public async Task AddBotToRoomAsync(int botId, int roomId)
        {
            var existing = await _context.Set<BotRoom>()
                .FirstOrDefaultAsync(br => br.BotId == botId && br.RoomId == roomId);

            if (existing != null)
            {
                existing.IsActive = true;
                existing.AddedAt = DateTime.UtcNow;
            }
            else
            {
                var botRoom = new BotRoom
                {
                    BotId = botId,
                    RoomId = roomId,
                    IsActive = true,
                    AddedAt = DateTime.UtcNow
                };
                _context.Set<BotRoom>().Add(botRoom);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveBotFromRoomAsync(int botId, int roomId)
        {
            var botRoom = await _context.Set<BotRoom>()
                .FirstOrDefaultAsync(br => br.BotId == botId && br.RoomId == roomId);

            if (botRoom != null)
            {
                botRoom.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Room>> GetBotRoomsAsync(int botId)
        {
            return await _context.Set<BotRoom>()
                .Where(br => br.BotId == botId && br.IsActive)
                .Include(br => br.Room)
                .ThenInclude(r => r.Channel)
                .ThenInclude(c => c.Server)
                .Select(br => br.Room)
                .ToListAsync();
        }

        // --- Utility ---
        public async Task<string> GenerateUniqueBotTokenAsync()
        {
            string token;
            do
            {
                token = GenerateSecureToken();
            } while (await _context.Bots.AnyAsync(b => b.Token == token));

            return token;
        }

        private static string GenerateSecureToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32]; // 256 bits
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
