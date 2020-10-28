using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service DeleteConfigurationSetAsync Unit Tests")]
    public class WebhooksServiceDeleteConfigurationSetAsyncTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceDeleteConfigurationSetContentAsyncForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildWebhooksService();

            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteConfigurationSetAsync(ContentIdForConfigurationSetDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceDeleteConfigurationSetContentAsyncForCreateReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var service = BuildWebhooksService();

            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteConfigurationSetAsync(ContentIdForConfigurationSetDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }
    }
}
