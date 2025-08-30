using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.Data.Repositories;
using DiscordClone.Services.ServerServices.Interface;
using DiscordClone.Services.ServerServices;
using DiscordClone.Services.UserServices.Interface;
using DiscordClone.Services.UserServices;
using DiscordClone.Services.MessageServices.Interface;
using DiscordClone.Services.MessageServices;
using DiscordClone.Services.RoomServices.Interface;
using DiscordClone.Services.RoomServices;
using DiscordClone.Services.ChannelService.Interface;
using DiscordClone.Services.ChannelService;

namespace DiscordClone
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IServerRepository, ServerRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            // ... thêm repo khác
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUserQueryService, UserQueryService>();
            services.AddScoped<IUserStatusService, UserStatusService>();
            services.AddScoped<IServerManagementService, ServerManagementService>();
            services.AddScoped<IServerMemberService,ServerMemberService>();
            services.AddScoped<IServerInviteService, ServerInviteService>();
            services.AddScoped<IMessageManagementService, MessageManagementService>();
            services.AddScoped<IMessageQueryService, MessageQueryService>();
            services.AddScoped<IRoomManagementService, RoomManagementService>();
            services.AddScoped<IRoomQueryService, RoomQueryService>();
            services.AddScoped<IChannelManagementService, ChannelManagementService>();
            services.AddScoped<IChannelQueryService, ChannelQueryService>();
            // ... thêm service khác
            return services;
        }
    }

}
