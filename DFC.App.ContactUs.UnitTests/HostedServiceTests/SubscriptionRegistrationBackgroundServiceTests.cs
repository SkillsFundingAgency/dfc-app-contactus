using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.HostedServices;
using DFC.App.ContactUs.Services.ApiProcessorService.UnitTests.FakeHttpHandlers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.HostedServiceTests
{
    public class SubscriptionRegistrationBackgroundServiceTests
    {
        private readonly IConfiguration configuration = A.Fake<IConfiguration>();
        private readonly WebhookSettings webhookSettings = new WebhookSettings();
        private readonly IHttpClientFactory httpClientFactory = A.Fake<IHttpClientFactory>();
        private readonly ILogger<SubscriptionRegistrationBackgroundService> logger = A.Fake<ILogger<SubscriptionRegistrationBackgroundService>>();

        public SubscriptionRegistrationBackgroundServiceTests()
        {

        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoApplicationNameSettingThrowsException()
        {
            //Arrange
            var serviceToTest = new SubscriptionRegistrationBackgroundService(configuration, webhookSettings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);

            serviceToTest.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoWebhookSettingThrowsException()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            var serviceToTest = new SubscriptionRegistrationBackgroundService(configuration, webhookSettings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);

            serviceToTest.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsSuccessful()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            webhookSettings.WebhookReceiverEndpoint = new Uri("https://somewebhookreceiver.com/receive");
            webhookSettings.SubscriptionsApiBaseAddress = new Uri("https://somewheretosubscribeto.com");

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationBackgroundService(configuration, webhookSettings, httpClientFactory, logger);

            //Act
            await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            serviceToTest.Dispose();
            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsDownstreamError()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            webhookSettings.WebhookReceiverEndpoint = new Uri("https://somewebhookreceiver.com/receive");
            webhookSettings.SubscriptionsApiBaseAddress = new Uri("https://somewheretosubscribeto.com");

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationBackgroundService(configuration, webhookSettings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            serviceToTest.Dispose();
            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }
    }
}
