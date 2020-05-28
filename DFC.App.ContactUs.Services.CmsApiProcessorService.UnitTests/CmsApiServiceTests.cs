using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.HttpClientPolicies;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService.UnitTests
{
    public class CmsApiServiceTests
    {
        private const string ValidPostcode = "CV1 1CV";

        private readonly IApiDataProcessorService fakeApiDataProcessorService = A.Fake<IApiDataProcessorService>();
        private readonly HttpClient fakeHttpClient = A.Fake<HttpClient>();

        private CmsApiClientOptions CmsApiClientOptions
        {
            get
            {
                return new CmsApiClientOptions
                {
                    BaseAddress = new Uri("https://localhost/", UriKind.Absolute),
                    SummaryEndpoint = "api/something",
                };
            }
        }

        [Fact]
        public async Task CmsApiServiceGetSummaryReturnsSuccess()
        {
            // arrange
            var expectedResults = A.CollectionOfFake<ContactUsSummaryItemModel>(2);

            A.CallTo(() => fakeApiDataProcessorService.GetAsync<IList<ContactUsSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResults);

            var cmsApiService = new CmsApiService(CmsApiClientOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await cmsApiService.GetSummaryAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<IList<ContactUsSummaryItemModel>>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResults);
        }

        [Fact]
        public async Task CmsApiServiceGetItemReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<ContactUsApiDataModel>();
            var url = new Uri($"{CmsApiClientOptions.BaseAddress}api/someitem", UriKind.Absolute);
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            var cmsApiService = new CmsApiService(CmsApiClientOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await cmsApiService.GetItemAsync(url).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<ContactUsApiDataModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
