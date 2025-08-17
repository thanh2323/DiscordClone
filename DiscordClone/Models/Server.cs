using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace DiscordClone.Models
{
    public class Server
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Icon { get; set; }

        [Required]
        public string AdminId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? InviteCode { get; set; }

        // Navigation properties
        public  User Admin { get; set; } = null!;
        public  ICollection<Channel> Channels { get; set; } = new List<Channel>();
        public  ICollection<ServerMember> Members { get; set; } = new List<ServerMember>();
        public  ICollection<Bot> Bots { get; set; } = new List<Bot>();

    }
}