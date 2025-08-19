    using DiscordClone.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace DiscordClone.Data.Configurations
    {
        public class RoomMemberConfiguration : IEntityTypeConfiguration<RoomMember>
        {
            public void Configure(EntityTypeBuilder<RoomMember> builder)
            {
               builder.HasKey(rm => new {rm.UserId, rm.RoomId });

                builder.HasOne(rm => rm.User)
                    .WithMany(u => u.RoomMembers)
                    .HasForeignKey(rm => rm.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(rm => rm.Room)
                    .WithMany(r =>r.RoomMembers)
                    .HasForeignKey(rm => rm.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
