using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.Services.BotService.Interface;

namespace DiscordClone.Services.BotService
{
    public class BotLifecycleService : IBotLifecycleService
    {
        private readonly IBotRepository _botRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public BotLifecycleService(IBotRepository botRepository, IMapper mapper, IServerRepository serverRepository, IRoomRepository roomRepository)
        {
            _serverRepository = serverRepository;
            _botRepository = botRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<BotDto?> ActivateAsync(int id)
        {
            var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return null;

            bot.IsActive = true;
            await _botRepository.UpdateAsync(bot);

            return _mapper.Map<BotDto>(bot);
        }

        public async Task<BotDto?> DeactivateAsync(int id)
        {
            var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return null;

            bot.IsActive = false;
            await _botRepository.UpdateAsync(bot);

            return _mapper.Map<BotDto>(bot);
        }
    }
}
