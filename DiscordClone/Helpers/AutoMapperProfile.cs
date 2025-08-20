using AutoMapper;
using DiscordClone.Models;
using DiscordClone.DTOs;
using DiscordClone.Models.DiscordClone.Models;
namespace DiscordClone.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty));

            CreateMap<CreateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Server mappings
            CreateMap<Server, ServerDto>()
                .ForMember(dest => dest.AdminName, opt => opt.MapFrom(src => src.Admin.DisplayName))
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count))
                .ForMember(dest => dest.Channels, opt => opt.MapFrom(src => src.Channels))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

            CreateMap<CreateServerDto, Server>()
                   .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                   .ForMember(dest => dest.InviteCode, opt => opt.Ignore());

            CreateMap<UpdateServerDto, Server>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ServerMember mappings
            CreateMap<ServerMember, ServerMemberDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom(src => src.User.IsOnline));

            // Channel mappings
            CreateMap<Channel, ChannelDto>()
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

            CreateMap<CreateChannelDto, Channel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Position, opt => opt.Ignore());

            CreateMap<UpdateChannelDto, Channel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
           
            // Room mappings
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.Channel.Name))
                .ForMember(dest => dest.CurrentUsers, opt => opt.MapFrom(src => 0)) // Will be calculated separately
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
                .ForMember(dest => dest.ActiveBots, opt => opt.MapFrom(src => src.BotRooms
                    .Where(br => br.IsActive)
                    .Select(br => br.Bot)));

            CreateMap<CreateRoomDto, Room>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdateRoomDto, Room>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Message mappings
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.ReplyToMessage, opt => opt.MapFrom(src => src.ReplyToMessage))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));

            CreateMap<CreateMessageDto, Message>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Will be set in service

            CreateMap<UpdateMessageDto, Message>()
                .ForMember(dest => dest.EditedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Bot mappings
            CreateMap<Bot, BotDto>()
                .ForMember(dest => dest.ActiveRoomIds, opt => opt.MapFrom(src => src.BotRooms
                    .Where(br => br.IsActive)
                    .Select(br => br.RoomId)));

            CreateMap<CreateBotDto, Bot>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdateBotDto, Bot>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // BotRoom mappings
            CreateMap<BotRoom, BotRoomDto>();
            CreateMap<BotRoomDto, BotRoom>()
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        }
    }
}
