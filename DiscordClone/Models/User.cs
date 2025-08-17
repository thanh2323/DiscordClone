using DiscordClone.Models.DiscordClone.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DiscordClone.Models
{
    public class User : IdentityUser
    {
   
        public string DisplayName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsOnline { get; set; }

        public DateTime? LastOnline { get; set; } = DateTime.UtcNow;

        public ICollection<Server> OwnedServers { get; set; } = new List<Server>();

        public ICollection<ServerMember> ServerMembersShips { get; set; } = new List<ServerMember>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();
   
    }
}
