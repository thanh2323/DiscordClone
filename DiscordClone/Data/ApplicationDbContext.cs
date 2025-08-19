using DiscordClone.Data.Configurations;
using DiscordClone.Models;
using DiscordClone.Models.DiscordClone.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
    
        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerMember> ServerMembers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Bot> Bots { get; set; }
        public DbSet<BotRoom> BotRooms { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply configurations
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ServerConfiguration());
            builder.ApplyConfiguration(new ServerMemberConfiguration());
            builder.ApplyConfiguration(new ChannelConfiguration());
            builder.ApplyConfiguration(new RoomConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new BotConfiguration());
            builder.ApplyConfiguration(new BotRoomConfiguration());
            builder.ApplyConfiguration(new RoomMemberConfiguration());
        }
    }
}
