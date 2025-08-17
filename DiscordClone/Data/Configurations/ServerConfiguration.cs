using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Icon)
                .HasMaxLength(255);

            builder.Property(s => s.AdminId)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.InviteCode)
                .HasMaxLength(50);

            // Relationships

            builder.HasOne(s => s.Admin)
                .WithMany(u => u.OwnedServers)
                .HasForeignKey(s => s.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Channels)
                .WithOne(c => c.Server)
                .HasForeignKey(c => c.ServerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Members)
                .WithOne(sm => sm.Server)
                .HasForeignKey(sm => sm.ServerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Bots)
              .WithOne(b => b.Server)
              .HasForeignKey(b => b.ServerId)
              .OnDelete(DeleteBehavior.Cascade);

            // Indexes

            builder.HasIndex(s => s.InviteCode).IsUnique();
            builder.HasIndex(s => s.AdminId);

        }
    }
}