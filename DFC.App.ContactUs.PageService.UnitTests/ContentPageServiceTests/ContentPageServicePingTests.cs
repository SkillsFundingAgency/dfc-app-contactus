using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServicePingTests
    {
        [Fact]
        public void ContentPageServicePingReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
