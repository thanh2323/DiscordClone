using AutoMapper;
using DiscordClone.Data.Repositories;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Models;
using DiscordClone.Services.ChannelService.Interface;

namespace DiscordClone.Services.ChannelService
{
    public class ChannelManagementService : IChannelManagementService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public ChannelManagementService(IChannelRepository channelRepository, IMapper mapper, IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ChannelDto>> CreateAsync(CreateChannelDto createChannelDto, string userId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(createChannelDto.ServerId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<ChannelDto>.ErrorResult("Access denied. Only admins can create channels.");
                }
                var channel = _mapper.Map<Channel>(createChannelDto);
                var createdChannel = await _channelRepository.AddAsync(channel);
                var channelDto = _mapper.Map<ChannelDto>(createdChannel);
                return ApiResponse<ChannelDto>.SuccessResult(channelDto, "Channel created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChannelDto>.ErrorResult("An error occurred while creating the channel", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int channelId, string userId)
        {
            try
            {

                var channel = await _channelRepository.GetByIdAsync(channelId);
                if (channel == null)
                    return ApiResponse<bool>.ErrorResult("Channel not found");


                var isAdmin = await _serverRepository.IsUserAdminAsync(channel.ServerId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("Access denied. Only admins can delete channels.");
                }
                await _channelRepository.DeleteAsync(channel);

                return ApiResponse<bool>.SuccessResult(true, "Channel deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Error deleting channel", ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ReorderAsync(int serverId, ReorderChannelsDto reorderDto, string userId)
        {
            try
            {
                var isAdmin = await _serverRepository.IsUserAdminAsync(serverId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<bool>.ErrorResult("Access denied. Only admins can reorder channels.");
                }
                await _channelRepository.ReorderChannelsAsync(serverId, reorderDto.ChannelPositions);
                return ApiResponse<bool>.SuccessResult(true, "Channels reordered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("An error occurred while reordering channels", ex.Message);
            }
        }

        public async Task<ApiResponse<ChannelDto>> UpdateAsync(int channelId, UpdateChannelDto updateChannelDto, string userId)
        {
            try
            {
                var channel = await _channelRepository.GetByIdAsync(channelId);
                if (channel == null)
                {
                    return ApiResponse<ChannelDto>.ErrorResult("Channel not found");
                }
                var isAdmin = await _serverRepository.IsUserAdminAsync(channel.ServerId, userId);
                if (!isAdmin)
                {
                    return ApiResponse<ChannelDto>.ErrorResult("Access denied. Only admins can update channels.");
                }
                _mapper.Map(updateChannelDto, channel);
                await _channelRepository.UpdateAsync(channel);
                var updatedChannelDto = _mapper.Map<ChannelDto>(channel);
                return ApiResponse<ChannelDto>.SuccessResult(updatedChannelDto, "Channel updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChannelDto>.ErrorResult("An error occurred while updating the channel", ex.Message);
            }

        }
    }
}
