using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.Models;
using DiscordClone.Services.BotService.Interface;

namespace DiscordClone.Services.BotService
{
    public class BotQueryService : IBotQueryService
    {
        private readonly IBotRepository _botRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public BotQueryService(IBotRepository botRepository, IMapper mapper, IServerRepository serverRepository, IRoomRepository roomRepository)
        {
            _serverRepository = serverRepository;
            _botRepository = botRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }
        public async Task<IEnumerable<BotDto>> GetActiveBotsAsync()
        {
            var bots = await _botRepository.GetActiveBotsAsync();
            return _mapper.Map<IEnumerable<BotDto>>(bots);
        }

        public async Task<IEnumerable<RoomDto>> GetBotRoomsAsync(int botId)
        {
            var rooms = await _botRepository.GetBotRoomsAsync(botId);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<BotDto?> GetByIdAsync(int id)
        {
            var bot = await _botRepository.GetByIdAsync(id);
            return _mapper.Map<BotDto?>(bot);
        }

        public async Task<BotDto?> GetByTokenAsync(string token)
        {
            var bot = await _botRepository.GetByTokenAsync(token);
            return _mapper.Map<BotDto?>(bot);
        }

        public async Task<IEnumerable<BotDto>> GetServerBotsAsync(int serverId)
        {
            var bots = await _botRepository.GetServerBotsAsync(serverId);
            return _mapper.Map<IEnumerable<BotDto>>(bots);
        }

        public async Task<BotDto?> GetWithRoomsAsync(int botId)
        {
            var bot = await _botRepository.GetWithRoomsAsync(botId);
            if (bot == null) return null;
            return _mapper.Map<BotDto>(bot);
        }

        public async Task<bool> IsBotInRoomAsync(int botId, int roomId)
        {
            return await _botRepository.IsBotInRoomAsync(botId, roomId);
        }
    }
}
