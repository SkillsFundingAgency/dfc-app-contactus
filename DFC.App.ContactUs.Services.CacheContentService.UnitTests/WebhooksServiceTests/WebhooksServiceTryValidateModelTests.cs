using DFC.App.ContactUs.Data.Models;
using System;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service TryValidateModel Unit Tests")]
    public class WebhooksServiceTryValidateModelTests : BaseWebhooksServiceTests
    {
        [Fact]
        public void WebhooksServiceTryValidateModelForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            var expectedValidEmailModel = BuildValidEmailModel();
            var service = BuildWebhooksService();

            // Act
            var result = service.TryValidateModel(expectedValidEmailModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateModelForUpdateReturnsFailure()
        {
            // Arrange
            const bool expectedResponse = false;
            var expectedInvalidEmailModel = new EmailModel();
            var service = BuildWebhooksService();

            // Act
            var result = service.TryValidateModel(expectedInvalidEmailModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateModelRaisesExceptionForNullEmailModel()
        {
            // Arrange
            EmailModel? nullEmailModel = null;
            var service = BuildWebhooksService();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => service.TryValidateModel(nullEmailModel));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'emailModel')", exceptionResult.Message);
        }
    }
}
