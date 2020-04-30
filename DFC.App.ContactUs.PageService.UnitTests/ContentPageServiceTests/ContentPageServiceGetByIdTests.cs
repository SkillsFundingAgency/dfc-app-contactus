using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetByIdTests
    {
        [Fact]
        public void ContentPageServiceGetByIdReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = A.Fake<ContentPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageServiceGetByIdReturnsNullWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            ContentPageModel? expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
