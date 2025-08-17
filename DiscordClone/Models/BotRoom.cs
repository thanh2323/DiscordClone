namespace DiscordClone.Models
{
    public class BotRoom
    {
        public int BotId { get; set; }
        public int RoomId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  Bot Bot { get; set; } = null!;
        public  Room Room { get; set; } = null!;
    }
}