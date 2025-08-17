/*using Microsoft.AspNetCore.Identity;
using DiscordClone.Models;
using DiscordClone.Data;
using DiscordClone.Models.DiscordClone.Models;
namespace DiscordClone.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DbSeeder(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Users.Any()) {
                await SeedUsersAsync();
            }
            if (!_context.Servers.Any())
            {
                await SeedServersAsync();
            }

            // Seed Channels
            if (!_context.Channels.Any())
            {
                await SeedChannelsAsync();
            }

            // Seed Rooms
            if (!_context.Rooms.Any())
            {
                await SeedRoomsAsync();
            }

            // Seed Messages
            if (!_context.Messages.Any())
            {
                await SeedMessagesAsync();
            }

            await _context.SaveChangesAsync();
        }
        private async Task SeedUsersAsync()
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin@discord.com",
                    Email = "admin@discord.com",
                    DisplayName = "Admin User",
                    EmailConfirmed = true,
                    IsOnline = true
                },
                new User
                {
                    UserName = "john@discord.com",
                    Email = "john@discord.com",
                    DisplayName = "John Doe",
                    EmailConfirmed = true,
                    IsOnline = false
                },
                new User
                {
                    UserName = "jane@discord.com",
                    Email = "jane@discord.com",
                    DisplayName = "Jane Smith",
                    EmailConfirmed = true,
                    IsOnline = true
                }
            };

            foreach (var user in users)
            {
                await _userManager.CreateAsync(user, "Password123!");
            }
        }

        private async Task SeedServersAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin@discord.com");
            var johnUser = await _userManager.FindByEmailAsync("john@discord.com");

            if (adminUser != null && johnUser != null)
            {
                var servers = new List<Server>
                {
                    new Server
                    {
                        Name = "Gaming Community",
                        Description = "A place for gamers to chat and play together",
                        AdminId = adminUser.Id,
                        InviteCode = "GAME123"
                    },
                    new Server
                    {
                        Name = "Study Group",
                        Description = "Learning and studying together",
                        AdminId = johnUser.Id,
                        InviteCode = "STUDY456"
                    }
                };

                await _context.Servers.AddRangeAsync(servers);
                await _context.SaveChangesAsync();

                // Add server members
                var serverMembers = new List<ServerMember>
                {
                    new ServerMember { UserId = adminUser.Id, ServerId = 1, Role = ServerRole.Admin },
                    new ServerMember { UserId = johnUser.Id, ServerId = 1, Role = ServerRole.Member },
                    new ServerMember { UserId = johnUser.Id, ServerId = 2, Role = ServerRole.Admin },
                };

                await _context.ServerMembers.AddRangeAsync(serverMembers);
            }
        }

        private async Task SeedChannelsAsync()
        {
            var channels = new List<Channel>
            {
                // Gaming Community Channels
                new Channel { Name = "general", Type = ChannelType.Text, ServerId = 1, Position = 0 },
                new Channel { Name = "gaming-chat", Type = ChannelType.Text, ServerId = 1, Position = 1 },
                new Channel { Name = "voice-hangout", Type = ChannelType.Voice, ServerId = 1, Position = 2 },
                new Channel { Name = "stream-zone", Type = ChannelType.Stream, ServerId = 1, Position = 3 },
                
                // Study Group Channels
                new Channel { Name = "announcements", Type = ChannelType.Text, ServerId = 2, Position = 0 },
                new Channel { Name = "homework-help", Type = ChannelType.Text, ServerId = 2, Position = 1 },
                new Channel { Name = "study-room", Type = ChannelType.Voice, ServerId = 2, Position = 2 }
            };

            await _context.Channels.AddRangeAsync(channels);
        }

        private async Task SeedRoomsAsync()
        {
            var rooms = new List<Room>
            {
                // General Channel Rooms
                new Room { Name = "welcome", Type = RoomType.Chat, ChannelId = 1 },
                new Room { Name = "random-chat", Type = RoomType.Chat, ChannelId = 1 },
                
                // Gaming Chat Channel Rooms
                new Room { Name = "valorant", Type = RoomType.Chat, ChannelId = 2 },
                new Room { Name = "cs-go", Type = RoomType.Chat, ChannelId = 2 },
                new Room { Name = "league-of-legends", Type = RoomType.Chat, ChannelId = 2 },
                
                // Voice Hangout Channel Rooms
                new Room { Name = "general-voice", Type = RoomType.Voice, ChannelId = 3, MaxUsers = 10 },
                new Room { Name = "gaming-voice", Type = RoomType.Voice, ChannelId = 3, MaxUsers = 8 },
                
                // Stream Zone Channel Rooms
                new Room { Name = "main-stream", Type = RoomType.Stream, ChannelId = 4, MaxUsers = 100 },
                new Room { Name = "community-stream", Type = RoomType.Stream, ChannelId = 4, MaxUsers = 50 },
                
                // Study Group Rooms
                new Room { Name = "important-news", Type = RoomType.Chat, ChannelId = 5 },
                new Room { Name = "math-help", Type = RoomType.Chat, ChannelId = 6 },
                new Room { Name = "programming-help", Type = RoomType.Chat, ChannelId = 6 },
                new Room { Name = "group-study", Type = RoomType.Voice, ChannelId = 7, MaxUsers = 15 }
            };

            await _context.Rooms.AddRangeAsync(rooms);
        }

        private async Task SeedMessagesAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin@discord.com");
            var johnUser = await _userManager.FindByEmailAsync("john@discord.com");
            var janeUser = await _userManager.FindByEmailAsync("jane@discord.com");

            if (adminUser != null && johnUser != null && janeUser != null)
            {
                var messages = new List<Message>
                {
                    // Welcome room messages
                    new Message
                    {
                        Content = "Welcome to our Gaming Community! 🎮",
                        UserId = adminUser.Id,
                        RoomId = 1,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-24)
                    },
                    new Message
                    {
                        Content = "Thanks for the invite! Excited to be here 😊",
                        UserId = johnUser.Id,
                        RoomId = 1,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-23)
                    },
                    new Message
                    {
                        Content = "Hello everyone! Ready to game? 🚀",
                        UserId = janeUser.Id,
                        RoomId = 1,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-22)
                    },
                    
                    // Valorant room messages
                    new Message
                    {
                        Content = "Anyone up for some Valorant? Looking for teammates!",
                        UserId = johnUser.Id,
                        RoomId = 3,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-12)
                    },
                    new Message
                    {
                        Content = "I'm in! What rank are you?",
                        UserId = janeUser.Id,
                        RoomId = 3,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-11)
                    },
                    new Message
                    {
                        Content = "Diamond 2, you?",
                        UserId = johnUser.Id,
                        RoomId = 3,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-10)
                    },
                    
                    // Study group messages
                    new Message
                    {
                        Content = "Study session starts at 7 PM tonight. Don't forget! 📚",
                        UserId = johnUser.Id,
                        RoomId = 10,
                        Type = MessageType.System,
                        CreatedAt = DateTime.UtcNow.AddHours(-6)
                    },
                    new Message
                    {
                        Content = "Can someone help me with calculus derivatives?",
                        UserId = janeUser.Id,
                        RoomId = 11,
                        Type = MessageType.Text,
                        CreatedAt = DateTime.UtcNow.AddHours(-3)
                    }
                };

                await _context.Messages.AddRangeAsync(messages);
            }
        }
    }
}
    

*/