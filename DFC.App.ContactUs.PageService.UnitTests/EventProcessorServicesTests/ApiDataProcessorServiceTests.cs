using DFC.App.ContactUs.PageService.EventProcessorServices;
using DFC.App.ContactUs.PageService.EventProcessorServices.Models;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.EventProcessorServicesTests
{
    [Trait("Category", "API Data Processor Service Unit Tests")]
    public class ApiDataProcessorServiceTests
    {
        private readonly ICmsApiProcessorService fakeCmsApiProcessorService = A.Fake<ICmsApiProcessorService>();

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsSuccess()
        {
            // arrange
            var expectedResult = new ContactUsSummaryItemModel
            {
                Url = new Uri("https://somewhere.com"),
                CanonicalName = "a-name",
                Published = DateTime.Now,
            };
            var jsonResponse = JsonConvert.SerializeObject(expectedResult);

            A.CallTo(() => fakeCmsApiProcessorService.GetDataFromApiAsync(A<Uri>.Ignored, A<string>.Ignored)).Returns(jsonResponse);

            var apiDataProcessorService = new ApiDataProcessorService(fakeCmsApiProcessorService);

            // act
            var result = await apiDataProcessorService.GetAsync<ContactUsSummaryItemModel>(new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiProcessorService.GetDataFromApiAsync(A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsNullForNoData()
        {
            // arrange
            ContactUsSummaryItemModel? expectedResult = null;

            A.CallTo(() => fakeCmsApiProcessorService.GetDataFromApiAsync(A<Uri>.Ignored, A<string>.Ignored)).Returns(string.Empty);

            var apiDataProcessorService = new ApiDataProcessorService(fakeCmsApiProcessorService);

            // act
            var result = await apiDataProcessorService.GetAsync<ContactUsSummaryItemModel>(new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiProcessorService.GetDataFromApiAsync(A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
