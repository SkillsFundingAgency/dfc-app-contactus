using DFC.App.ContactUs.Data.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.IntegrationTests.Fakes
{
    public class FakeConfigurationSetReloadService : IConfigurationSetReloadService
    {
        public Task Reload(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
