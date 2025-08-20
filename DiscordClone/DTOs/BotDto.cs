using DiscordClone.Models.DiscordClone.Models;

namespace DiscordClone.DTOs
{
    public class BotDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public int ServerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> ActiveRoomIds { get; set; } = new();
    }

    public class CreateBotDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string Token { get; set; } = string.Empty;
        public string? WebhookUrl { get; set; }
        public int ServerId { get; set; }
    }

    public class UpdateBotDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? WebhookUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class BotMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public MessageType Type { get; set; } = MessageType.Bot;
    }

    public class BotRoomDto
    {
        public int BotId { get; set; }
        public int RoomId { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
