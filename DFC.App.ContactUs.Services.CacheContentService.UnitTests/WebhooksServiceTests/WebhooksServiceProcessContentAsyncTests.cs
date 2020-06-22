﻿//using DFC.App.ContactUs.Data.Models;
//using FakeItEasy;
//using System;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
//{
//    [Trait("Category", "Webhooks Service ProcessContentAsync Unit Tests")]
//    public class WebhooksServiceProcessContentAsyncTests : BaseWebhooksServiceTests
//    {
//        [Fact]
//        public async Task WebhooksServiceProcessContentAsyncForCreateReturnsSuccess()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
//            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
//            var expectedValidContentPageModel = BuildValidContentPageModel();
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.Created);

//            // Act
//            var result = await service.ProcessContentAsync(url, ContentIdForCreate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }

//        [Fact]
//        public async Task WebhooksServiceProcessContentAsyncForUpdateReturnsSuccess()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
//            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
//            var expectedValidContentPageModel = BuildValidContentPageModel();
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

//            // Act
//            var result = await service.ProcessContentAsync(url, ContentIdForCreate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }

//        [Fact]
//        public async Task WebhooksServiceProcessContentAsyncForUpdateReturnsNoContent()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.NoContent;
//            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
//            ContentPageModel? expectedValidContentPageModel = default;
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
//            A.CallTo(() => FakeMapper.Map<ContentPageModel?>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);

//            // Act
//            var result = await service.ProcessContentAsync(url, ContentIdForCreate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }

//        [Fact]
//        public async Task WebhooksServiceProcessContentAsyncForUpdateReturnsBadRequest()
//        {
//            // Arrange
//            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
//            var expectedValidApiContentModel = BuildValidContactUsApiContentModel();
//            var expectedValidContentPageModel = new ContentPageModel();
//            var url = new Uri("https://somewhere.com");
//            var service = BuildWebhooksService();

//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidApiContentModel);
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);

//            // Act
//            var result = await service.ProcessContentAsync(url, ContentIdForCreate).ConfigureAwait(false);

//            // Assert
//            A.CallTo(() => FakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
//            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
//            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

//            Assert.Equal(expectedResponse, result);
//        }
//    }
//}
