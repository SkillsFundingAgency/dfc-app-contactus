using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.PageService.EventProcessorServices.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WebhooksControllerTests
{
    [Trait("Category", "Webhooks Controller Unit Tests")]
    public class WebhooksControllerPostTests : BaseWebhooksController
    {
        public static IEnumerable<object[]> PublishedEvents => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json, EventTypePublished },
            new object[] { MediaTypeNames.Application.Json, EventTypeDraft },
        };

        public static IEnumerable<object[]> DeletedEvents => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json, EventTypeDeleted },
        };

        public static IEnumerable<object[]> InvalidIdValues => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { "Not a Guid" },
        };

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishCreatePostReturnsSuccess(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiModel = BuildValidContactUsApiDataModel();
            var expectedValidModel = BuildValidContentPageModel();
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(mediaTypeName);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidApiModel);
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishUpdatePostReturnsSuccess(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiModel = BuildValidContactUsApiDataModel();
            var expectedValidModel = BuildValidContentPageModel();
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(mediaTypeName);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidApiModel);
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(DeletedEvents))]
        public async Task WebhooksControllerDeletePostReturnsSuccess(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(mediaTypeName);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishUpdatePostReturnsAlreadyReported(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var expectedValidApiModel = BuildValidContactUsApiDataModel();
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(mediaTypeName);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).Returns(expectedValidApiModel);
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.AlreadyReported);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidIdValues))]
        public async Task WebhooksControllerPostReturnsErrorForInvalidId(string id)
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent(EventTypePublished, new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(MediaTypeNames.Application.Json);
            eventGridEvents.First().Id = id;
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveContactUsEvents().ConfigureAwait(false)).ConfigureAwait(false);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();

            controller.Dispose();
        }

        [Fact]
        public async Task WebhooksControllerPostReturnsSuccessForUnknownEventType()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent("Unknown", new EventGridEventData { Api = "https://somewhere.com" });
            var controller = BuildWebhooksController(MediaTypeNames.Application.Json);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Fact]
        public async Task WebhooksControllerPostReturnsErrorForInvalidUrl()
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent(EventTypePublished, new EventGridEventData { Api = "http:http://badUrl" });
            var controller = BuildWebhooksController(MediaTypeNames.Application.Json);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveContactUsEvents().ConfigureAwait(false)).ConfigureAwait(false);

            A.CallTo(() => FakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();

            controller.Dispose();
        }

        [Fact]
        public async Task WebhooksControllerSubscriptionvalidationReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            string expectedValidationCode = Guid.NewGuid().ToString();
            var eventGridEvents = BuildValidEventGridEvent(Microsoft.Azure.EventGrid.EventTypes.EventGridSubscriptionValidationEvent, new SubscriptionValidationEventData(expectedValidationCode, "https://somewhere.com"));
            var controller = BuildWebhooksController(MediaTypeNames.Application.Json);
            controller.HttpContext.Request.Body = BuildStreamFromEventGridEvents(eventGridEvents);

            // Act
            var result = await controller.ReceiveContactUsEvents().ConfigureAwait(false);

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<SubscriptionValidationResponse>(jsonResult.Value);

            Assert.Equal((int)expectedResponse, jsonResult.StatusCode);
            Assert.Equal(expectedValidationCode, response.ValidationResponse);

            controller.Dispose();
        }

        private static ContactUsApiDataModel BuildValidContactUsApiDataModel()
        {
            var model = new ContactUsApiDataModel()
            {
                Id = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                Content = "<h1>A document</h1>",
                Published = DateTime.UtcNow,
            };

            return model;
        }

        private static ContentPageModel BuildValidContentPageModel()
        {
            var model = new ContentPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                Content = "<h1>A document</h1>",
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        private static EventGridEvent[] BuildValidEventGridEvent<TModel>(string eventType, TModel data)
        {
            var models = new EventGridEvent[]
            {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"{ContentTypeContactUs}/a-canonical-name",
                    Data = data,
                    EventType = eventType,
                    EventTime = DateTime.Now,
                    DataVersion = "1.0",
                },
            };

            return models;
        }

        private static Stream BuildStreamFromEventGridEvents(EventGridEvent[] eventGridEvents)
        {
            var jsonData = JsonConvert.SerializeObject(eventGridEvents);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            MemoryStream stream = new MemoryStream(byteArray);

            return stream;
        }
    }
}
