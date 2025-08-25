using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.Models;
using DiscordClone.Services.BotService.Interface;

namespace DiscordClone.Services.BotService
{
    public class BotManagementService : IBotManagementService
    {
        private readonly IBotRepository _botRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public BotManagementService(IBotRepository botRepository, IMapper mapper, IServerRepository serverRepository, IRoomRepository roomRepository)
        {
            _serverRepository = serverRepository;
            _botRepository = botRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<bool> AddToRoomAsync(int botId, int roomId)
        {
            var bot = await _botRepository.GetByIdAsync(botId);
            if (bot == null || !bot.IsActive) return false;

            // Validate room exists and is active
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null || !room.IsActive) return false;

            // Check if bot is already in room
            var isAlreadyInRoom = await _botRepository.IsBotInRoomAsync(botId, roomId);
            if (isAlreadyInRoom) return false;

            await _botRepository.AddBotToRoomAsync(botId, roomId);
            return true;
        }

        public async Task<BotDto> CreateAsync(CreateBotDto createBotDto)
        {
            var server = _serverRepository.GetByIdAsync(createBotDto.ServerId);
            if (server == null) throw new Exception("Server not found");
            var bot = _mapper.Map<Bot>(createBotDto);
            bot.Token = await _botRepository.GenerateUniqueBotTokenAsync();
            bot.CreatedAt = DateTime.UtcNow;
            bot.IsActive = true;

            await _botRepository.AddAsync(bot);

            return _mapper.Map<BotDto>(bot);
        }

        public async Task<bool> DeleteAsync(int id)
        {
           var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return false;
            return await _botRepository.DeleteAsync(id);
        }

        public async Task<string> RegenerateTokenAsync(int botId)
        {
            var bot = await _botRepository.GetByIdAsync(botId);
            if (bot == null)
                throw new ArgumentException("Bot not found");

            bot.Token = await _botRepository.GenerateUniqueBotTokenAsync();
             await _botRepository.UpdateAsync(bot);

            return bot.Token;
        }

        public async Task<bool> RemoveFromRoomAsync(int botId, int roomId)
        {
            var isInRoom = await _botRepository.IsBotInRoomAsync(botId, roomId);
            if (!isInRoom) return false;

            await _botRepository.RemoveBotFromRoomAsync(botId, roomId);
            return true;
        }

        public async Task<BotDto?> UpdateAsync(int id, UpdateBotDto updateBotDto)
        {
            var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return null;
            _mapper.Map(updateBotDto, bot);
            await _botRepository.UpdateAsync(bot);

            return _mapper.Map<BotDto>(bot);
        }
    }
}
