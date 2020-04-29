using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.MessageFunctionApp.Services;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.MessageFunctionApp.UnitTests.Services
{
    [Trait("Messaging Function", "Message Processor Tests")]
    public class MessageProcessorTests
    {
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;
        private readonly IMessageProcessor messageProcessor;

        public MessageProcessorTests()
        {
            httpClientService = A.Fake<IHttpClientService>();
            mappingService = A.Fake<IMappingService>();

            messageProcessor = new MessageProcessor(httpClientService, mappingService);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileCreatePublishedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.Created;
            const string message = "{}";
            const long sequenceNumber = 1;
            const MessageContentType messageContentType = MessageContentType.Pages;

            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).Returns(A.Fake<ContentPageModel>());
            A.CallTo(() => httpClientService.PutAsync(A<ContentPageModel>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PutAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileUpdatePublishedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;
            const MessageContentType messageContentType = MessageContentType.Pages;

            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).Returns(A.Fake<ContentPageModel>());
            A.CallTo(() => httpClientService.PutAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => httpClientService.PostAsync(A<ContentPageModel>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PutAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PostAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileDeletedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;
            const MessageContentType messageContentType = MessageContentType.Pages;

            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).Returns(A.Fake<ContentPageModel>());
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Deleted).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncWithBadMessageMessageActionReturnsException()
        {
            // arrange
            const string message = "{}";
            const long sequenceNumber = 1;
            const MessageContentType messageContentType = MessageContentType.Pages;

            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).Returns(A.Fake<ContentPageModel>());

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, (MessageAction)(-1)).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToContentPageModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            Assert.Equal("Invalid message action '-1' received, should be one of 'Published,Deleted,Draft' (Parameter 'messageAction')", exceptionResult.Message);
        }

        [Fact]
        public async Task ProcessAsyncWithBadMessageContentTypeReturnsException()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await messageProcessor.ProcessAsync(string.Empty, 1, (MessageContentType)(-1), MessageAction.Published).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Unexpected content type '-1' (Parameter 'messageContentType')", exceptionResult.Message);
        }
    }
}
