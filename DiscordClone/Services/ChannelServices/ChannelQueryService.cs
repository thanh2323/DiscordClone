using AutoMapper;
using DiscordClone.Data.Repositories.IRepositories;
using DiscordClone.DTOs;
using DiscordClone.DTOs.Common;
using DiscordClone.Services.ChannelService.Interface;

namespace DiscordClone.Services.ChannelService
{
    public class ChannelQueryService : IChannelQueryService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public ChannelQueryService(IChannelRepository channelRepository, IMapper mapper, IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ChannelDto>> GetChannelByIdAsync(int channelId, string userId)
        {
            try
            {
                var channel = await _channelRepository.GetWithRoomsAsync(channelId);
                if (channel == null)
                {
                    return ApiResponse<ChannelDto>.ErrorResult("Channel not found");
                }

                // Check if user is member of the server
                var isMember = await _serverRepository.IsUserMemberAsync(channel.ServerId, userId);
                if (!isMember)
                {
                    return ApiResponse<ChannelDto>.ErrorResult("Access denied");
                }

                var channelDto = _mapper.Map<ChannelDto>(channel);
                return ApiResponse<ChannelDto>.SuccessResult(channelDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<ChannelDto>.ErrorResult("Error retrieving channel", ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<ChannelDto>>> GetServerChannelsAsync(int serverId, string userId)
        {
            try
            {
                var isMember = await _serverRepository.IsUserMemberAsync(serverId, userId);
                if (!isMember)
                {
                    return ApiResponse<IEnumerable<ChannelDto>>.ErrorResult("Access denied");
                }

                var channels = await _channelRepository.GetServerChannelsAsync(serverId);
                var channelDtos = _mapper.Map<IEnumerable<ChannelDto>>(channels);
                return ApiResponse<IEnumerable<ChannelDto>>.SuccessResult(channelDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ChannelDto>>.ErrorResult("Error retrieving server channels", ex.Message);
            }
        }
    }
}
