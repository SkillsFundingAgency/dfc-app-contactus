using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetByNameTests
    {
        private const string CanonicalName = "name1";
        private readonly ICosmosRepository<ContentPageModel> repository;
        private readonly IContentPageService contentPageService;

        public ContentPageServiceGetByNameTests()
        {
            repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            contentPageService = new ContentPageService(repository);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<ContentPageModel>();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await contentPageService.GetByNameAsync(CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsArgumentNullExceptionWhenNullNameIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'canonicalName')", exceptionResult.Message);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsNullWhenMissingRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns((ContentPageModel)null);

            // act
            var result = await contentPageService.GetByNameAsync(CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}