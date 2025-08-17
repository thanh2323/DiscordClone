using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class BotConfiguration : IEntityTypeConfiguration<Bot>
    {
        public void Configure(EntityTypeBuilder<Bot> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(500);

            builder.Property(b => b.Avatar)
                .HasMaxLength(255);

            builder.Property(b => b.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.WebhookUrl)
                .HasMaxLength(500);

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(b => b.Server)
                .WithMany(s => s.Bots)
                .HasForeignKey(b => b.ServerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.BotRooms)
                .WithOne(br => br.Bot)
                .HasForeignKey(br => br.BotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(b => b.ServerId);
            builder.HasIndex(b => b.Token).IsUnique();
        }
    }
}