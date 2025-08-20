using DiscordClone.Models;

namespace DiscordClone.DTOs
{
    public class ServerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string AdminId { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? InviteCode { get; set; }
        public int MemberCount { get; set; }
        public List<ChannelDto> Channels { get; set; } = new();
        public List<ServerMemberDto> Members { get; set; } = new();
    }

    public class CreateServerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class UpdateServerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class ServerMemberDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public ServerRole Role { get; set; }
        public bool IsOnline { get; set; }
        public DateTime JoinedAt { get; set; }
    }
    public class JoinServerDto
    {
        public string InviteCode { get; set; } = string.Empty;
    }

}
