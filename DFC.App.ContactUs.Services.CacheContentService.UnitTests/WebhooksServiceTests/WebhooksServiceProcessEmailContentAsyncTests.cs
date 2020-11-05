using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service ProcessEmailContentAsync Unit Tests")]
    public class WebhooksServiceProcessEmailContentAsyncTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceProcessEmailContentAsyncForCreateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = BuildValidEmailModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessEmailContentAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessEmailContentAsyncForUpdateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = BuildValidEmailModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessEmailContentAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessEmailContentAsyncForUpdateReturnsNoContent()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            EmailModel? expectedValidEmailModel = default;
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel?>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);

            // Act
            var result = await service.ProcessEmailContentAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessEmailContentAsyncForUpdateReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = new EmailModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);

            // Act
            var result = await service.ProcessEmailContentAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }
    }
}
