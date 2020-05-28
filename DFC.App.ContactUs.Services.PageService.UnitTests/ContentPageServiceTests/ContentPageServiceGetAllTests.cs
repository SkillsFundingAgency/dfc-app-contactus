using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetAllTests
    {
        [Fact]
        public async Task ContentPageServiceGetAllListReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResults = A.CollectionOfFake<ContentPageModel>(2);

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = await contentPageService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public async Task ContentPageServiceGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            IEnumerable<ContentPageModel>? expectedResults = null;

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = await contentPageService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }
    }
}
