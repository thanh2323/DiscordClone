using System.ComponentModel.DataAnnotations;
using DiscordClone.Models.DiscordClone.Models;

namespace DiscordClone.Models
{
    public enum RoomType
    {
        Chat = 0,
        Voice = 1,
        Stream = 2
    }

    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public RoomType Type { get; set; } = RoomType.Chat;

        public int ChannelId { get; set; }

        public int MaxUsers { get; set; } = 50;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  Channel Channel { get; set; } = null!;
        public  ICollection<Message> Messages { get; set; } = new List<Message>();
        public  ICollection<BotRoom> BotRooms { get; set; } = new List<BotRoom>();
    }
}