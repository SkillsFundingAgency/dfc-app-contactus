using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.IntegrationTests.Fakes
{
    public class FakeConfigurationSetBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
