using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.DisplayName)
                 .IsRequired()
                 .HasMaxLength(100);
            builder.Property(u => u.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.LastOnline)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships

            builder.HasMany(u => u.OwnedServers)
                .WithOne(s => s.Admin)
                .HasForeignKey(s => s.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ServerMembersShips)
                .WithOne(sm => sm.User)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}