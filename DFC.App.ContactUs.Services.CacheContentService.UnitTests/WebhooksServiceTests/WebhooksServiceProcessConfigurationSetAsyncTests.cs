using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service ProcessConfigurationSetAsync Unit Tests")]
    public class WebhooksServiceProcessConfigurationSetAsyncTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceProcessConfigurationSetAsyncForCreateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessConfigurationSetAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessConfigurationSetAsyncForUpdateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessConfigurationSetAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessConfigurationSetAsyncForUpdateReturnsNoContent()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            ConfigurationSetModel? expectedValidConfigurationSetModel = default;
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel?>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);

            // Act
            var result = await service.ProcessConfigurationSetAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessConfigurationSetAsyncForUpdateReturnsBadRequest()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            var expectedValidConfigurationSetModel = new ConfigurationSetModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);

            // Act
            var result = await service.ProcessConfigurationSetAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }
    }
}
