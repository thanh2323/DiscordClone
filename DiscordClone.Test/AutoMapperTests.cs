using AutoMapper;
using DiscordClone.Helpers;
using Xunit;

namespace DiscordClone.Test
{
    public class AutoMapperTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        public AutoMapperTests()
        {
            // Load toàn bộ Profile bạn đã định nghĩa
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            _configuration.AssertConfigurationIsValid();
        }
    }
}
