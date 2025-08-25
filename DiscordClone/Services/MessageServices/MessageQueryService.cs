using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.MessageServices.Interface;

namespace DiscordClone.Services.MessageServices
{
    public class MessageQueryService : IMessageQueryService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public MessageQueryService(IMessageRepository messageRepository, IRoomRepository roomRepository, IServerRepository serverRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _roomRepository = roomRepository;
            _serverRepository = serverRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<MessageDto>> GetMessageByIdAsync(int messageId, string userId)
        {
            try
            {
                var message =  await _messageRepository.GetMessageWithRoomAndUserAsync(messageId);
                if (message == null)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Message not found");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(message.Room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Access denied");
                }
                var messageDto = _mapper.Map<MessageDto>(message);
                return ApiResponse<MessageDto>.SuccessResult(messageDto);

            }
            catch (Exception ex)
            {
                return ApiResponse<MessageDto>.ErrorResult("An error occurred while retrieving the message", ex.Message);
            }
        }

        public async Task<ApiResponse<PagedResult<MessageDto>>> GetRoomMessagesAsync(int roomId, string userId, PaginationParams pagination)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<PagedResult<MessageDto>>.ErrorResult("Room not found");
                }

                var isMember = await _serverRepository.IsUserMemberAsync(room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<PagedResult<MessageDto>>.ErrorResult("Access denied");

                }
                var messages = await _messageRepository.GetRoomMessagesAsync(roomId, pagination.PageSize, pagination.Skip);
                var totalCount = await _messageRepository.CountAsync(m => m.RoomId == roomId && !m.IsDeleted);

                var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
                var pagedResult = new PagedResult<MessageDto>(messageDtos, totalCount, pagination.Page, pagination.PageSize);

                return ApiResponse<PagedResult<MessageDto>>.SuccessResult(pagedResult, "Messages retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<MessageDto>>.ErrorResult("An error occurred while retrieving messages", ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<MessageDto>>> SearchMessagesAsync(MessageSearchDto searchDto, string userId)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(searchDto.RoomId);
                if (room == null)
                {
                    return ApiResponse<IEnumerable<MessageDto>>.ErrorResult("Room not found");
                }
                if (room == null)
                    return ApiResponse<IEnumerable<MessageDto>>.ErrorResult("Room not found");

               
                var isMember = await _serverRepository.IsUserMemberAsync(room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<IEnumerable<MessageDto>>.ErrorResult("Access denied");
                }
                var messages = await _messageRepository.SearchMessagesAsync(searchDto.RoomId, searchDto.SearchTerm);
                var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

                return ApiResponse<IEnumerable<MessageDto>>.SuccessResult(messageDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<MessageDto>>.ErrorResult("An error occurred while searching messages", ex.Message);
            }
        }
    }
}
