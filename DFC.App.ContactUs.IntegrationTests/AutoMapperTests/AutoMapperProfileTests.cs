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

        [Fact]
<<<<<<< HEAD
        public void AutoMapperProfileConfigurationForContentPageModelProfileReturnSuccess()
=======
        public void AutoMapperProfileConfigurationForChatOptionsProfileReturnSuccess()
>>>>>>> story/DFCC-1169-refresh-nugets
        {
            // Arrange
            factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
<<<<<<< HEAD
            mapper.ConfigurationProvider.AssertConfigurationIsValid<ContentPageModelProfile>();
=======
            mapper.ConfigurationProvider.AssertConfigurationIsValid<ChatOptionsProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void AutoMapperProfileConfigurationForEmailModelProfileReturnSuccess()
        {
            // Arrange
            factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<EmailModelProfile>();
>>>>>>> story/DFCC-1169-refresh-nugets

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void AutoMapperProfileConfigurationForEnterYourDetailsBodyViewModelProfileReturnSuccess()
        {
            // Arrange
            factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<EnterYourDetailsBodyViewModelProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void AutoMapperProfileConfigurationForAllProfilesReturnSuccess()
        {
            // Arrange
            factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            Assert.True(true);
        }
    }
}
