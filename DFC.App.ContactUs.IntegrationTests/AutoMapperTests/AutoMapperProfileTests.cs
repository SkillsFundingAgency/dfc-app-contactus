using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DFC.App.ContactUs.IntegrationTests.AutoMapperTests
{
    [Trait("Category", "Integration - Automapper")]
    public class AutoMapperProfileTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public AutoMapperProfileTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact(Skip = "Needs code refactor")]
        public void AutoMapperProfileConfigurationForChatOptionsProfileReturnSuccess()
        {
            // Arrange
            factory.CreateClientWithWebHostBuilder();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<ChatOptionsProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact(Skip = "Needs code refactor")]
        public void AutoMapperProfileConfigurationForEConfigurationSetModelProfileReturnSuccess()
        {
            // Arrange
            factory.CreateClientWithWebHostBuilder();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<ConfigurationSetModelProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact(Skip = "Needs code refactor")]
        public void AutoMapperProfileConfigurationForEnterYourDetailsBodyViewModelProfileReturnSuccess()
        {
            // Arrange
            factory.CreateClientWithWebHostBuilder();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<EnterYourDetailsBodyViewModelProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact(Skip = "Needs code refactor")]
        public void AutoMapperProfileConfigurationForAllProfilesReturnSuccess()
        {
            // Arrange
            factory.CreateClientWithWebHostBuilder();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            Assert.True(true);
        }
    }
}
