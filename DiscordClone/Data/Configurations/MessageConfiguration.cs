using DiscordClone.Models.DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Content)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(m => m.Type)
                .HasConversion<int>()
                .HasDefaultValue(MessageType.Text);

            builder.Property(m => m.FileUrl)
                .HasMaxLength(500);

            builder.Property(m => m.FileName)
                .HasMaxLength(255);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing for replies
            builder.HasOne(m => m.ReplyToMessage)
                .WithMany(m => m.Replies)
                .HasForeignKey(m => m.ReplyToMessageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(m => m.RoomId);
            builder.HasIndex(m => m.UserId);
            builder.HasIndex(m => m.CreatedAt);
            builder.HasIndex(m => m.IsDeleted);
        }
    }
}