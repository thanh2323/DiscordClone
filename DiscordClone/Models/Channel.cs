using System.ComponentModel.DataAnnotations;

namespace DiscordClone.Models
{
    public enum ChannelType
    {
        Text = 0,
        Voice = 1,
        Stream = 2
    }

    public class Channel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ChannelType Type { get; set; } = ChannelType.Text;

        public int ServerId { get; set; }

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  Server Server { get; set; } = null!;
        public  ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
