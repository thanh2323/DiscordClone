using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    public class ServerMemberConfiguration : IEntityTypeConfiguration<ServerMember>
    {
        public void Configure(EntityTypeBuilder<ServerMember> builder)
        {

            // Composite primary key
            builder.HasKey(sm => new { sm.ServerId, sm.UserId });

            builder.Property(sm => sm.Role)
                .HasConversion<int>()
                .HasDefaultValue(ServerRole.Member);


            builder.Property(sm => sm.JoinedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(sm => sm.User)
                .WithMany(u => u.ServerMembersShips)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sm => sm.Server)
                .WithMany(s => s.Members)
                .HasForeignKey(sm => sm.ServerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}