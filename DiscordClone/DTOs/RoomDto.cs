using DiscordClone.Models;

namespace DiscordClone.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public RoomType Type { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public int MaxUsers { get; set; }
        public int CurrentUsers { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
        public List<BotDto> ActiveBots { get; set; } = new();
    }

    public class CreateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public RoomType Type { get; set; } = RoomType.Chat;
        public int ChannelId { get; set; }
        public int MaxUsers { get; set; } = 50;
    }

    public class UpdateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxUsers { get; set; }
        public bool IsActive { get; set; }
    }
}

