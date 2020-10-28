using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.HostedServices;
using DFC.Compui.Telemetry.HostedService;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.HostedServicesTests
{
    public class ConfigurationSetBackgroundServiceTests
    {
        private readonly IConfigurationSetReloadService configurationSetReloadService = A.Fake<IConfigurationSetReloadService>();
        private readonly ILogger<ConfigurationSetBackgroundService> logger = A.Fake<ILogger<ConfigurationSetBackgroundService>>();
        private readonly IHostedServiceTelemetryWrapper wrapper = A.Fake<IHostedServiceTelemetryWrapper>();

        [Fact]
        public async Task ConfigurationSetBackgroundServiceStartsAsyncCompletesSuccessfully()
        {
            // Arrange
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).Returns(Task.CompletedTask);
            var serviceToTest = new ConfigurationSetBackgroundService(logger, new CmsApiClientOptions { BaseAddress = new Uri("http://somewhere.com") }, configurationSetReloadService, wrapper);

            // Act
            await serviceToTest.StartAsync(default).ConfigureAwait(false);

            // Assert
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            serviceToTest.Dispose();
        }

        [Fact]
        public async Task ConfigurationSetBackgroundServiceStartsAsyncThrowsException()
        {
            // Arrange
            A.CallTo(() => wrapper.Execute(A<Func<Task>>.Ignored, A<string>.Ignored)).Returns(Task.FromException(new Exception("An Exception")));
            var serviceToTest = new ConfigurationSetBackgroundService(logger, new CmsApiClientOptions { BaseAddress = new Uri("http://somewhere.com") }, configurationSetReloadService, wrapper);

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await serviceToTest.StartAsync(default).ConfigureAwait(false)).ConfigureAwait(false);
            serviceToTest.Dispose();
        }
    }
}
