namespace DiscordClone.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace DiscordClone.Models
    {
        public enum MessageType
        {
            Text = 0,
            Image = 1,
            File = 2,
            System = 3,
            Bot = 4
        }

        public class Message
        {
            public int Id { get; set; }

            [Required]
            public string Content { get; set; } = string.Empty;

            public MessageType Type { get; set; } = MessageType.Text;

            public string UserId { get; set; } = string.Empty;

            public int RoomId { get; set; }

            public string? FileUrl { get; set; }

            public string? FileName { get; set; }

            public long? FileSize { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime? EditedAt { get; set; }

            public bool IsDeleted { get; set; } = false;

            // For reply functionality (optional)
            public int? ReplyToMessageId { get; set; }

            // Navigation properties
            public  User User { get; set; } = null!;
            public  Room Room { get; set; } = null!;
            public  Message? ReplyToMessage { get; set; }
            public  ICollection<Message> Replies { get; set; } = new List<Message>();
        }
    }
}