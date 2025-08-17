using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Type)
                .HasConversion<int>()
                .HasDefaultValue(RoomType.Chat);

            builder.Property(r => r.MaxUsers)
                .HasDefaultValue(50);

            builder.Property(r => r.IsActive)
                .HasDefaultValue(true);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(r => r.Channel)
                .WithMany(c => c.Rooms)
                .HasForeignKey(r => r.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Messages)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.BotRooms)
                .WithOne(br => br.Room)
                .HasForeignKey(br => br.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(r => r.ChannelId);
            builder.HasIndex(r => r.IsActive);
        }
    }
}