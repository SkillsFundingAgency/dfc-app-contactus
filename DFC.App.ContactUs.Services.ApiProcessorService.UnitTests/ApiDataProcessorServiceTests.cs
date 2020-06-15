using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.ApiProcessorService.UnitTests
{
    [Trait("Category", "API Data Processor Service Unit Tests")]
    public class ApiDataProcessorServiceTests
    {
        private readonly IApiService fakeApiService = A.Fake<IApiService>();

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

            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).Returns(jsonResponse);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.GetAsync<ContactUsSummaryItemModel>(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsNullForNoData()
        {
            // arrange
            ContactUsSummaryItemModel? expectedResult = null;

            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).Returns(string.Empty);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.GetAsync<ContactUsSummaryItemModel>(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
