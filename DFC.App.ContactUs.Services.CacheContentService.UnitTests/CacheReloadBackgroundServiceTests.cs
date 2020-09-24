using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.HostedServices;
using DFC.Compui.Telemetry.HostedService;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    public class CacheReloadBackgroundServiceTests
    {
        private readonly IEmailCacheReloadService emailCacheReloadService = A.Fake<IEmailCacheReloadService>();
        private readonly ILogger<CacheReloadBackgroundService> logger = A.Fake<ILogger<CacheReloadBackgroundService>>();
        private readonly IHostedServiceTelemetryWrapper wrapper = A.Fake<IHostedServiceTelemetryWrapper>();

        [Fact]
        public async Task CacheReloadBackgroundServiceStartsAsyncCompletesSuccessfully()
        {
            // Arrange
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).Returns(Task.CompletedTask);
            var serviceToTest = new CacheReloadBackgroundService(logger, new CmsApiClientOptions { BaseAddress = new Uri("http://somewhere.com") }, emailCacheReloadService, wrapper);

            // Act
            await serviceToTest.StartAsync(default).ConfigureAwait(false);

            // Assert
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            serviceToTest.Dispose();
        }

        [Fact]
        public async Task CacheReloadBackgroundServiceStartsAsyncThrowsException()
        {
            // Arrange
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).Returns(Task.FromException(new Exception("An Exception")));
            var serviceToTest = new CacheReloadBackgroundService(logger, new CmsApiClientOptions { BaseAddress = new Uri("http://somewhere.com") }, emailCacheReloadService, wrapper);

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await serviceToTest.StartAsync(default).ConfigureAwait(false)).ConfigureAwait(false);
            serviceToTest.Dispose();
        }
    }
}
