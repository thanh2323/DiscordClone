using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Services.RoomServices.Interface;

namespace DiscordClone.Services.RoomServices
{
    public class RoomQueryService : IRoomQueryService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public RoomQueryService(IRoomRepository roomRepository, IMapper mapper, IChannelRepository channelRepository, IServerRepository serverRepository)
        {
            _channelRepository = channelRepository;
            _serverRepository = serverRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<RoomDto>>> GetChannelRoomsAsync(int channelId, string userId)
        {
            try
            {
                var channel = await _channelRepository.GetByIdAsync(channelId);
                if (channel == null)
                {
                    return ApiResponse<IEnumerable<RoomDto>>.ErrorResult("Channel not found");
                }

                // Check if user is member of the server
                var isMember = await _serverRepository.IsUserMemberAsync(channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<IEnumerable<RoomDto>>.ErrorResult("Access denied");
                }

                var rooms = await _roomRepository.GetChannelRoomsAsync(channelId);
                var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);
                return ApiResponse<IEnumerable<RoomDto>>.SuccessResult(roomDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<RoomDto>>.ErrorResult("Error retrieving rooms", ex.Message);
            }
        }

        public async Task<ApiResponse<RoomDto>> GetRoomByIdAsync(int roomId, string userId)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Room not found");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Access denied");
                }

                var roomDto = _mapper.Map<RoomDto>(room);
                return ApiResponse<RoomDto>.SuccessResult(roomDto);
            }

            catch (Exception ex)
            {
                return ApiResponse<RoomDto>.ErrorResult("Error retrieving room", ex.Message);
            }
        }

        public async Task<ApiResponse<RoomDto>> GetRoomWithMessagesAsync(int roomId, string userId, PaginationParams pagination)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Room not found");
                }

                // Check if user is member of the server
                var isMember = await _serverRepository.IsUserMemberAsync(room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Access denied");
                }
                var roomWithMessages = await _roomRepository.GetWithMessagesAsync(roomId, pagination.PageSize, pagination.Skip);
                var roomDto = _mapper.Map<RoomDto>(roomWithMessages);
                return ApiResponse<RoomDto>.SuccessResult(roomDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<RoomDto>.ErrorResult("Error retrieving room with messages", ex.Message);
            }

        }
    }
}
