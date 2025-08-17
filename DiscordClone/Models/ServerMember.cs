namespace DiscordClone.Models
{
    public enum ServerRole
    {
        Member = 0,
        Admin = 1
    }

    public class ServerMember
    {
        public string UserId { get; set; } = string.Empty;
        public int ServerId { get; set; }

        public ServerRole Role { get; set; } = ServerRole.Member;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  User User { get; set; } = null!;
        public  Server Server { get; set; } = null!;
    }
}
