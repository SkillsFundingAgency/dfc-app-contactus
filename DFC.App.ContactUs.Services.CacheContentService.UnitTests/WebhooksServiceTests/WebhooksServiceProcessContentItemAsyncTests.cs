﻿//using DFC.App.ContactUs.Data.Models;
//using FakeItEasy;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
//{
//    [Trait("Category", "Webhooks Service ProcessContentItemAsync Unit Tests")]
//    public class WebhooksServiceProcessContentItemAsyncTests : BaseWebhooksServiceTests
//    {
//        [Fact]
//        public async Task WebhooksServiceProcessContentItemAsyncForCreateReturnsSuccess()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
//            var exptectedGuidList = new List<Guid> { ContentIdForCreate, Guid.NewGuid() };
//            var expectedValidApiContentItemModel = BuildValidContactUsApiContentItemDataModel();
//            var expectedValidContentPageModel = BuildValidContentPageModel();
//            var expectedValidContentItemModel = BuildValidContentItemModel(ContentItemIdForCreate);
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(exptectedGuidList);
//            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentItemModel);
//            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedValidContentPageModel);
//            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).Returns(expectedValidContentItemModel);
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

//            // Act
//            var result = await service.ProcessContentItemAsync(url, ContentItemIdForCreate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }

//        [Fact]
//        public async Task WebhooksServiceProcessContentItemAsyncForUpdateReturnsSuccess()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
//            var exptectedGuidList = new List<Guid> { ContentIdForUpdate, Guid.NewGuid() };
//            var expectedValidApiContentItemModel = BuildValidContactUsApiContentItemDataModel();
//            var expectedValidContentPageModel = BuildValidContentPageModel();
//            var expectedValidContentItemModel = BuildValidContentItemModel(ContentItemIdForUpdate);
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(exptectedGuidList);
//            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentItemModel);
//            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedValidContentPageModel);
//            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).Returns(expectedValidContentItemModel);
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

//            // Act
//            var result = await service.ProcessContentItemAsync(url, ContentItemIdForUpdate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceOrMore();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }

//        [Fact]
//        public async Task WebhooksServiceProcessContentItemAsyncForUpdateReturnsNoContent()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
//            var exptectedEmptyGuidList = new List<Guid>();
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).Returns(exptectedEmptyGuidList);

//            // Act
//            var result = await service.ProcessContentItemAsync(url, ContentItemIdForUpdate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeContentCacheService.GetContentIdsContainingContentItemId(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeCmsApiService.GetContentItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeMapper.Map(A<ContactUsApiContentItemModel>.Ignored, A<ContentItemModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }
//    }
//}
