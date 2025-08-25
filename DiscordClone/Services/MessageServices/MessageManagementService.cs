using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models.DiscordClone.Models;
using DiscordClone.Services.MessageServices.Interface;

namespace DiscordClone.Services.MessageServices
{
    public class MessageManagementService : IMessageManagementService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public MessageManagementService(IMessageRepository messageRepository, IRoomRepository roomRepository, IServerRepository serverRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _roomRepository = roomRepository;
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> DeleteMessageAsync(int messageId, string userId)
        {
            try
            {
                var message = await _messageRepository.GetByIdAsync(messageId);
                if (message == null)
                {
                    return ApiResponse<bool>.ErrorResult("Message not found");
                }
                if (message.UserId != userId)
                {
                    return ApiResponse<bool>.ErrorResult("You do not have permission to delete this message");
                }
                var result = await _messageRepository.DeleteAsync(message);
                if (!result)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to delete message");
                }
                return ApiResponse<bool>.SuccessResult(true, "Message deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("An error occurred while deleting the message", ex.Message);
            }
        }

        public async Task<ApiResponse<MessageDto>> SendMessageAsync(CreateMessageDto createMessageDto, string userId)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(createMessageDto.RoomId);
                if (room == null)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Room not found");
                }
                var isMember = await _serverRepository.IsUserMemberAsync(room.Channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Access denied");
                }
                var message = _mapper.Map<Message>(createMessageDto);
                message.UserId = userId;
                message.CreatedAt = DateTime.UtcNow;
                var addedMessage = await _messageRepository.AddAsync(message);

                var messageDto = _mapper.Map<MessageDto>(addedMessage);
                return ApiResponse<MessageDto>.SuccessResult(messageDto, "Message sent successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MessageDto>.ErrorResult("An error occurred while sending the message", ex.Message);
            }
        }

        public async Task<ApiResponse<MessageDto>> UpdateMessageAsync(int messageId, UpdateMessageDto updateMessageDto, string userId)
        {
            try
            {
                var message = await _messageRepository.GetByIdAsync(messageId);
                if (message == null)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Message not found");
                }
                if (message.UserId != userId)
                {
                    return ApiResponse<MessageDto>.ErrorResult("You do not have permission to update this message");
                }
                _mapper.Map(updateMessageDto, message);
                var updatedMessage = await _messageRepository.UpdateAsync(message);
                if (!updatedMessage)
                {
                    return ApiResponse<MessageDto>.ErrorResult("Failed to update message");
                }
                var messageDto = _mapper.Map<MessageDto>(message);
                return ApiResponse<MessageDto>.SuccessResult(messageDto, "Message updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MessageDto>.ErrorResult("An error occurred while updating the message", ex.Message);
            }
        }
    }
}
