using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.IntegrationTests.Fakes;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.EmailService;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DFC.App.ContactUs.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
            MockSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            MockDocumentService = A.Fake<IDocumentService<StaticContentItemModel>>();
        }

        internal ISessionStateService<SessionDataModel> MockSessionStateService { get; set; }

        internal IDocumentService<StaticContentItemModel> MockDocumentService { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder?.ConfigureServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true, reloadOnChange: true)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(new CmsApiClientOptions()
                {
                    ApiKey = "123",
                    BaseAddress = new Uri("https://localhost:8081"),
                    ContentIds = Guid.NewGuid().ToString(),
                    StaticContentEndpoint = "/sharedcontent/",
                    Timeout = TimeSpan.FromSeconds(30)
                });

                services.AddTransient(sp => MockSessionStateService);

                services.AddTransient<INotifyClientProxy, FakeNotifyClientProxy>();
                services.AddTransient<IDocumentService<StaticContentItemModel>, FakeDocumentService>();
            });
        }
    }
}
