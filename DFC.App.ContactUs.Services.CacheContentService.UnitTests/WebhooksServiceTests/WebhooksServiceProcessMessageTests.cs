﻿using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service ProcessMessageAsync Unit Tests")]
    public class WebhooksServiceProcessMessageTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncNoneOptionReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), ContentIdForEmailCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentThrowsErrorForInvalidUrl()
        {
            // Arrange
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = BuildValidEmailModel();
            var url = "/somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForEmailCreate, url).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateEmailReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = BuildValidEmailModel();
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForEmailCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateConfigurationSetReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForConfigurationSetCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentUpdateEmailReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidEmailApiDataModel = BuildValidEmailApiDataModel();
            var expectedValidEmailModel = BuildValidEmailModel();
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidEmailApiDataModel);
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).Returns(expectedValidEmailModel);
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForEmailUpdate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentUpdateConfigurationSetReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidConfigurationSetApiDataModel = BuildValidConfigurationSetApiDataModel();
            var expectedValidConfigurationSetModel = BuildValidConfigurationSetModel();
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidConfigurationSetApiDataModel);
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).Returns(expectedValidConfigurationSetModel);
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForConfigurationSetUpdate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteEmailReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentIdForEmailDelete, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteConfigurationSetReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = "https://somewhere.com";
            var service = BuildWebhooksService();

            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentIdForConfigurationSetDelete, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailDocumentService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeConfigurationSetDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResponse, result);
        }
    }
}
