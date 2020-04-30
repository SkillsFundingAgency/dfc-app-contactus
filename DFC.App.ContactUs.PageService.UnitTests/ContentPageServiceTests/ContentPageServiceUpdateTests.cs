using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceUpdateTests
    {
        [Fact]
        public void ContentPageServiceUpdateReturnsSuccessWhenContentPageReplaced()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = A.Fake<ContentPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.UpsertAsync(contentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(contentPageModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async System.Threading.Tasks.Task ContentPageServiceUpdateReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            ContentPageModel? contentPageModel = null;
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageService = new ContentPageService(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.UpsertAsync(contentPageModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'contentPageModel')", exceptionResult.Message);
        }

        [Fact]
        public void ContentPageServiceUpdateReturnsNullWhenContentPageNotReplaced()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageModel = A.Fake<ContentPageModel>();
            var expectedResult = A.Dummy<ContentPageModel>();

            A.CallTo(() => repository.UpsertAsync(contentPageModel)).Returns(HttpStatusCode.BadRequest);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.UpsertAsync(contentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(contentPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageServiceUpdateReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            var contentPageModel = A.Fake<ContentPageModel>();
            ContentPageModel? expectedResult = null;

            A.CallTo(() => repository.UpsertAsync(contentPageModel)).Returns(HttpStatusCode.FailedDependency);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.UpsertAsync(contentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(contentPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}
