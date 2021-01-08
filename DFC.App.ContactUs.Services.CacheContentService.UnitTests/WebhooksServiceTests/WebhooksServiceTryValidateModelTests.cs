using DFC.App.ContactUs.Data.Models;
using System;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service TryValidateModel Unit Tests")]
    public class WebhooksServiceTryValidateModelTests : BaseWebhooksServiceTests
    {
        [Fact]
        public void WebhooksServiceTryValidateEmailModelForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var service = BuildWebhooksService();

            // Act
            var result = service.TryValidateModel(expectedValidConfigurationSetModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateConfigurationSetModelForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var service = BuildWebhooksService();

            // Act
            var result = service.TryValidateModel(expectedValidConfigurationSetModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateModelForUpdateReturnsFailure()
        {
            // Arrange
            const bool expectedResponse = false;
            var expectedValidConfigurationSetModel = new ConfigurationSetModel();
            var service = BuildWebhooksService();

            // Act
            var result = service.TryValidateModel(expectedValidConfigurationSetModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }
    }
}
