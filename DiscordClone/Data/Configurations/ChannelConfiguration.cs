using DiscordClone.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Data.Configurations
{
    internal class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
              .HasMaxLength(500);

            builder.Property(c => c.Type)
             .HasConversion<int>()
             .HasDefaultValue(ChannelType.Text);


            builder.Property(c => c.Position)
               .HasDefaultValue(0);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


            // Relationships
            builder.HasOne(c => c.Server)
                .WithMany(s => s.Channels)
                .HasForeignKey(c => c.ServerId)
                .OnDelete(DeleteBehavior.Cascade);
            // Indexes
            builder.HasIndex(c => c.ServerId);
            builder.HasIndex(c => new { c.ServerId, c.Position });
        }
    }
}