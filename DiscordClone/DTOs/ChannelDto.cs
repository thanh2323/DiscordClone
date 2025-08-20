using DiscordClone.Models;

namespace DiscordClone.DTOs
{
    public class ChannelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ChannelType Type { get; set; }
        public int ServerId { get; set; }
        public int Position { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RoomDto> Rooms { get; set; } = new();
    }

    public class CreateChannelDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ChannelType Type { get; set; } = ChannelType.Text;
        public int ServerId { get; set; }
    }

    public class UpdateChannelDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ChannelType Type { get; set; }
    }

    public class ReorderChannelsDto
    {
        public Dictionary<int, int> ChannelPositions { get; set; } = new();
    }
}
