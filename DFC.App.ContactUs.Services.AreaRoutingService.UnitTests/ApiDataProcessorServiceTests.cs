using DFC.App.ContactUs.Services.AreaRoutingService.UnitTests.Models;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Services.ApiProcessorService;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using DFC.App.ContactUs.Services.AreaRoutingService.UnitTests.Models;

namespace DFC.App.ContactUs.Services.AreaRoutingService.UnitTests
{
    [Trait("Category", "API Data Processor Service Unit Tests")]
    public class ApiDataProcessorServiceTests
    {
        private readonly IApiService fakeApiService = A.Fake<IApiService>();

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsSuccess()
        {
            // arrange
            var expectedResult = new ApiSummaryModel
            {
                Url = new Uri("https://somewhere.com"),
                Title = "a-name",
            };
            var jsonResponse = JsonConvert.SerializeObject(expectedResult);

            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).Returns(jsonResponse);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.GetAsync<ApiSummaryModel>(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsNullForNoData()
        {
            // arrange
            IApiDataModel? expectedResult = null;

            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).Returns(string.Empty);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.GetAsync<IApiDataModel>(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServiceGetReturnsExceptionForNoHttpClient()
        {
            // arrange
            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await apiDataProcessorService.GetAsync<IApiDataModel>
                    (null, new Uri("https://somewhere.com")).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.GetAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            Assert.Equal("Value cannot be null. (Parameter 'httpClient')", exceptionResult.Message);
        }

        [Fact]
        public async Task ApiDataProcessorServicePostReturnsSuccess()
        {
            // arrange
            var expectedResult = HttpStatusCode.Created;

            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.PostAsync(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServicePostReturnsExceptionForNoHttpClient()
        {
            // arrange
            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await apiDataProcessorService.PostAsync(null, new Uri("https://somewhere.com")).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
            Assert.Equal("Value cannot be null. (Parameter 'httpClient')", exceptionResult.Message);
        }

        [Fact]
        public async Task ApiDataProcessorServicePostWithModelReturnsSuccess()
        {
            // arrange
            var expectedResult = HttpStatusCode.Created;
            var fakeApiItemModel = A.Fake<ApiItemModel>();

            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<ApiItemModel>.Ignored)).Returns(expectedResult);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.PostAsync(A.Fake<HttpClient>(), new Uri("https://somewhere.com"), fakeApiItemModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<ApiItemModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServicePostWithModelReturnsExceptionForNoHttpClient()
        {
            // arrange
            var fakeApiItemModel = A.Fake<ApiItemModel>();

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await apiDataProcessorService.PostAsync(null, new Uri("https://somewhere.com"), fakeApiItemModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.PostAsync(A<HttpClient>.Ignored, A<Uri>.Ignored, A<ApiItemModel>.Ignored)).MustNotHaveHappened();
            Assert.Equal("Value cannot be null. (Parameter 'httpClient')", exceptionResult.Message);
        }

        [Fact]
        public async Task ApiDataProcessorServiceDeleteReturnsSuccess()
        {
            // arrange
            var expectedResult = HttpStatusCode.Created;

            A.CallTo(() => fakeApiService.DeleteAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var result = await apiDataProcessorService.DeleteAsync(A.Fake<HttpClient>(), new Uri("https://somewhere.com")).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.DeleteAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ApiDataProcessorServiceDeleteReturnsExceptionForNoHttpClient()
        {
            // arrange
            var apiDataProcessorService = new ApiDataProcessorService(fakeApiService);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await apiDataProcessorService.DeleteAsync(null, new Uri("https://somewhere.com")).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiService.DeleteAsync(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
            Assert.Equal("Value cannot be null. (Parameter 'httpClient')", exceptionResult.Message);
        }
    }
}
