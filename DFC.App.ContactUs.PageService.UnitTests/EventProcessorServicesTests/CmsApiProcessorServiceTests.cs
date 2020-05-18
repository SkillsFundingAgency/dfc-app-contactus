using DFC.App.ContactUs.PageService.EventProcessorServices;
using DFC.App.ContactUs.PageService.UnitTests.FakeHttpHandlers;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.PageService.UnitTests.EventProcessorServicesTests
{
    [Trait("Category", "CMS API Processor Service Unit Tests")]
    public class CmsApiProcessorServiceTests
    {
        private readonly ILogger<CmsApiProcessorService> logger;

        public CmsApiProcessorServiceTests()
        {
            logger = A.Fake<ILogger<CmsApiProcessorService>>();
        }

        [Fact]
        public async Task CmsApiProcessorServiceReturnsOkStatusCodeForValidUrl()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string expectedResponse = "Expected response string";
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult, Content = new StringContent(expectedResponse) };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);
            var cmsApiProcessorService = new CmsApiProcessorService(logger, httpClient);
            var url = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await cmsApiProcessorService.GetDataFromApiAsync(url, MediaTypeNames.Application.Json).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResponse, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task CmsApiProcessorServiceReturnsNotFoundStatusCode()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NotFound;
            string expectedResponse = string.Empty;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult, Content = new StringContent(expectedResponse) };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);
            var cmsApiProcessorService = new CmsApiProcessorService(logger, httpClient);
            var url = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await cmsApiProcessorService.GetDataFromApiAsync(url, MediaTypeNames.Application.Json).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task CmsApiProcessorServiceReturnsNoContentStatusCode()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            string expectedResponse = string.Empty;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult, Content = new StringContent(expectedResponse) };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);
            var cmsApiProcessorService = new CmsApiProcessorService(logger, httpClient);
            var url = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await cmsApiProcessorService.GetDataFromApiAsync(url, MediaTypeNames.Application.Json).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }
    }
}
