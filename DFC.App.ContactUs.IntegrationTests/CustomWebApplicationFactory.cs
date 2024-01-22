using DFC.App.ContactUs.IntegrationTests.Fakes;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.EmailService;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DFC.App.ContactUs.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
            MockSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            this.MockSharedContentRedis = new Mock<ISharedContentRedisInterface>();
        }

        public Mock<ISharedContentRedisInterface> MockSharedContentRedis { get; set; }

        internal ISessionStateService<SessionDataModel> MockSessionStateService { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder?.ConfigureServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient(sp => MockSessionStateService);

                services.AddTransient<INotifyClientProxy, FakeNotifyClientProxy>();

                services.AddScoped<ISharedContentRedisInterface>(_ => MockSharedContentRedis.Object);
            });
        }
    }
}
