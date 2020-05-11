using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.PageService.EventProcessorServices;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.EventProcessorServicesTests
{
    [Trait("Category", "Event Message Service Unit Tests")]
    public class EventMessageServiceTests
    {
        private readonly ILogger<EventMessageService> fakeLogger = A.Fake<ILogger<EventMessageService>>();
        private readonly IContentPageService fakeContentPageService = A.Fake<IContentPageService>();

        [Fact]
        public async Task EventMessageServiceGetAllCachedCanonicalNamesReturnsSuccess()
        {
            // arrange
            var expectedResult = A.CollectionOfFake<ContentPageModel>(2);

            A.CallTo(() => fakeContentPageService.GetAllAsync()).Returns(expectedResult);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.GetAllCachedCanonicalNamesAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsSuccess()
        {
            // arrange
            ContentPageModel? existingContentPageModel = null;
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingContentPageModel);
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).Returns(expectedResult);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsBadRequestWhenNullSupplied()
        {
            // arrange
            ContentPageModel? contentPageModel = null;
            var expectedResult = HttpStatusCode.BadRequest;

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceCreateAsyncReturnsAlreadyReportedWhenAlreadyExists()
        {
            // arrange
            var existingContentPageModel = A.Fake<ContentPageModel>();
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingContentPageModel);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsSuccess()
        {
            // arrange
            var existingContentPageModel = A.Fake<ContentPageModel>();
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = HttpStatusCode.OK;

            existingContentPageModel.Version = Guid.NewGuid();
            contentPageModel.Version = Guid.NewGuid();

            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingContentPageModel);
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).Returns(expectedResult);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsBadRequestWhenNullSupplied()
        {
            // arrange
            ContentPageModel? contentPageModel = null;
            var expectedResult = HttpStatusCode.BadRequest;

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsnotFoundWhenNotExists()
        {
            // arrange
            ContentPageModel? existingContentPageModel = null;
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingContentPageModel);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceUpdateAsyncReturnsAlreadyReportedWhenAlreadyExists()
        {
            // arrange
            var existingContentPageModel = A.Fake<ContentPageModel>();
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            existingContentPageModel.Version = Guid.NewGuid();
            contentPageModel.Version = existingContentPageModel.Version;

            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingContentPageModel);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceDeleteAsyncReturnsSuccess()
        {
            // arrange
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => fakeContentPageService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task EventMessageServiceDeleteAsyncReturnsNotFound()
        {
            // arrange
            var expectedResult = HttpStatusCode.NotFound;

            A.CallTo(() => fakeContentPageService.DeleteAsync(A<Guid>.Ignored)).Returns(false);

            var eventMessageService = new EventMessageService(fakeLogger, fakeContentPageService);

            // act
            var result = await eventMessageService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentPageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
