using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.RoomServices.Interface;

namespace DiscordClone.Services.RoomServices
{
    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public RoomManagementService(IRoomRepository roomRepository, IMapper mapper, IChannelRepository channelRepository, IServerRepository serverRepository)
        {
            _channelRepository = channelRepository;
            _serverRepository = serverRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<RoomDto>> CreateRoomAsync(CreateRoomDto createRoomDto, string userId)
        {
            try
            {
                var channel = await _channelRepository.GetByIdAsync(createRoomDto.ChannelId);
                if (channel == null)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Channel not found");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<RoomDto>.ErrorResult("You are not a member of this server");
                }

                var room = _mapper.Map<Room>(createRoomDto);
                await _roomRepository.AddAsync(room);

                var roomDto = _mapper.Map<RoomDto>(room);
                return ApiResponse<RoomDto>.SuccessResult(roomDto, "Room created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<RoomDto>.ErrorResult("An error occurred while creating the room", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteRoomAsync(int roomId, string userId)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<bool>.ErrorResult("Room not found");
                }
                var isAdmin = await _serverRepository.IsUserAdminAsync(room.Channel.ServerId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("Access denied. Only admins can delete rooms.");
                }
                var deleted = await _roomRepository.DeleteAsync(room);
                if (!deleted)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to delete room");
                }
                return ApiResponse<bool>.SuccessResult(true, "Room deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("An error occurred while deleting the room", ex.Message);
            }
        }

        public async Task<ApiResponse<RoomDto>> UpdateRoomAsync(int roomId, UpdateRoomDto updateRoomDto, string userId)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Room not found");
                }
                var isAdmin = await _serverRepository.IsUserAdminAsync(room.Channel.ServerId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Access denied. Only admins can update rooms.");
                }

                _mapper.Map(updateRoomDto, room);
                var updated = await _roomRepository.UpdateAsync(room);
                if (!updated)
                {
                    return ApiResponse<RoomDto>.ErrorResult("Failed to update room");
                }
                var roomDto = _mapper.Map<RoomDto>(room);
                return ApiResponse<RoomDto>.SuccessResult(roomDto, "Room updated successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<RoomDto>.ErrorResult("An error occurred while updating the room", ex.Message);
            }
        }
    }
}
