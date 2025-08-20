using DiscordClone.Models.DiscordClone.Models;

namespace DiscordClone.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserDisplayName { get; set; } = string.Empty;
        public string? UserAvatar { get; set; }
        public int RoomId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public int? ReplyToMessageId { get; set; }
        public MessageDto? ReplyToMessage { get; set; }
        public List<MessageDto> Replies { get; set; } = new();
    }

    public class CreateMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
        public int RoomId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public int? ReplyToMessageId { get; set; }
    }

    public class UpdateMessageDto
    {
        public string Content { get; set; } = string.Empty;
    }

    public class MessageSearchDto
    {
        public int RoomId { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
    }
}
