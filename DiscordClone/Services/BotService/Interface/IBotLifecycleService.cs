using DiscordClone.DTOs;

namespace DiscordClone.Services.BotService.Interface
{
    public interface IBotLifecycleService
    {
        Task<BotDto?> ActivateAsync(int id);
        Task<BotDto?> DeactivateAsync(int id);
    }
}
