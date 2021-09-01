using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.IntegrationTests.Fakes;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.EmailService;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.App.ContactUs.IntegrationTests.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder RegisterServices(
            this IWebHostBuilder webHostBuilder, ICosmosRepository<ConfigurationSetModel> cosmosRepository, ISessionStateService<SessionDataModel> sessionStateService)
        {
            return webHostBuilder.ConfigureTestServices(services =>
            {
                services.AddTransient<INotifyClientProxy, FakeNotifyClientProxy>();
                services.AddTransient<ISubscriptionRegistrationService, FakeSubscriptionRegistrationService>();
                services.AddTransient(sp => cosmosRepository);
                services.AddTransient(sp => sessionStateService);
                services.AddTransient<IConfigurationSetReloadService, FakeConfigurationSetReloadService>();
                services.AddTransient<IWebhooksService, FakeWebhooksService>();
            });
        }
    }
}
