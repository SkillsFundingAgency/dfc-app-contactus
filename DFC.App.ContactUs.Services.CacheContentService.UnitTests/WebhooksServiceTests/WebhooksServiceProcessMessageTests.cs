﻿using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Collections.Generic;
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
            const bool isContentItem = false;
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = false;
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentUpdateReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = false;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForUpdate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = false;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentIdForDelete, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentItemCreateReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = true;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiContentItemModel = BuildValidContactUsApiContentItemDataModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var expectedValidContentItemModel = BuildValidContentItemModel(ContentItemIdForCreate);
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(new List<Guid> { ContentIdForCreate, Guid.NewGuid() });
            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentItemModel);
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).Returns(expectedValidContentItemModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentItemIdForCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentItemUpdateReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = true;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiContentItemModel = BuildValidContactUsApiContentItemDataModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var expectedValidContentItemModel = BuildValidContentItemModel(ContentItemIdForUpdate);
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(new List<Guid> { ContentIdForUpdate, Guid.NewGuid() });
            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentItemModel);
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).Returns(expectedValidContentItemModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentItemIdForUpdate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentItemDeleteReturnsSuccess()
        {
            // Arrange
            const bool isContentItem = true;
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var url = new Uri("https://somewhere.com");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).Returns(isContentItem);
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(new List<Guid> { ContentIdForDelete, Guid.NewGuid() });
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentItemIdForDelete, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentCacheService.CheckIsContentItem(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceOrMore();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }
    }
}