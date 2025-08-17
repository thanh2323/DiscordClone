using System.ComponentModel.DataAnnotations;

namespace DiscordClone.Models
{
    public class Bot
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Avatar { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        public string? WebhookUrl { get; set; }

        public int ServerId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  Server Server { get; set; } = null!;
        public  ICollection<BotRoom> BotRooms { get; set; } = new List<BotRoom>();
    }
}
