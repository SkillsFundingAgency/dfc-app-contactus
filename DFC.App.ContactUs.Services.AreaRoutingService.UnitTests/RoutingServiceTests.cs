using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using FakeItEasy;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.AreaRoutingService.UnitTests
{
    public class RoutingServiceTests
    {
        private const string ValidPostcode = "CV1 1CV";

        private readonly IApiDataProcessorService fakeApiDataProcessorService = A.Fake<IApiDataProcessorService>();
        private readonly HttpClient fakeHttpClient = A.Fake<HttpClient>();

        private FamApiRoutingOptions FamApiRoutingOptions
        {
            get
            {
                return new FamApiRoutingOptions
                {
                    BaseAddress = new Uri("https://localhost/", UriKind.Absolute),
                    AreaRoutingEndpoint = "api/something",
                };
            }
        }

        [Fact]
        public async Task RoutingServiceGetRouteForPostcodeReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<RoutingDetailModel>();

            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(expectedResult);

            var routingService = new RoutingService(FamApiRoutingOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await routingService.GetAsync(ValidPostcode).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
