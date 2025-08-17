using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class BotRoomConfiguration : IEntityTypeConfiguration<BotRoom>
    {
        public void Configure(EntityTypeBuilder<BotRoom> builder)
        {
            // Composite primary key
            builder.HasKey(br => new { br.BotId, br.RoomId });

            builder.Property(br => br.IsActive)
                .HasDefaultValue(true);

            builder.Property(br => br.AddedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(br => br.Bot)
                .WithMany(b => b.BotRooms)
                .HasForeignKey(br => br.BotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(br => br.Room)
                .WithMany(r => r.BotRooms)
                .HasForeignKey(br => br.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}