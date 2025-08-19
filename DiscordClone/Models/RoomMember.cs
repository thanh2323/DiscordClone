namespace DiscordClone.Models
{
    public class RoomMember
    {

        public string UserId { get; set; } = null!;
        public int RoomId { get; set; }

        public bool IsActive { get; set; } = false;   // true nếu đang ở trong room (VD voice/stream)

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}
