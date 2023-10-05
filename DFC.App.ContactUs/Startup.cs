using AutoMapper;
using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.HttpClientPolicies;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services;
using DFC.App.ContactUs.Services.AreaRoutingService;
using DFC.App.ContactUs.Services.EmailService;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;

using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using DFC.Content.Pkg.Netcore.Services.CmsApiProcessorService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public const string StaticCosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:StaticContent";
        private const string CosmosDbSessionStateConfigAppSettings = "Configuration:CosmosDbConnections:SessionState";
        private const string NotifyOptionsAppSettings = "Configuration:NotifyOptions";

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        //public IConfiguration Configuration { get; }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // add the default route
                endpoints.MapControllerRoute("default", "{controller=Health}/{action=Ping}");
            });
            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cosmosDbConnectionSessionState = configuration.GetSection(CosmosDbSessionStateConfigAppSettings).Get<CosmosDbConnection>();
            services.AddSessionStateServices<SessionDataModel>(cosmosDbConnectionSessionState, env.IsDevelopment());

            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddSingleton<ValidationHtmlAttributeProvider, CustomValidationHtmlAttributeProvider>();
            services.AddTransient<INotifyEmailService<ContactUsEmailRequestModel>, NotifyEmailService<ContactUsEmailRequestModel>>();

            services.AddSingleton(configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            var staticContentDbConnection = configuration.GetSection(StaticCosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosRetryOptions = new RetryOptions { MaxRetryAttemptsOnThrottledRequests = 20, MaxRetryWaitTimeInSeconds = 60 };
            services.AddDocumentServices<StaticContentItemModel>(staticContentDbConnection, env.IsDevelopment(), cosmosRetryOptions);
            services.AddTransient<ICmsApiService, CmsApiService>();
            services.AddTransient<IStaticContentReloadService, StaticContentReloadService>();

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSingleton(configuration.GetSection(nameof(ChatOptions)).Get<ChatOptions>() ?? new ChatOptions());
            services.AddSingleton(configuration.GetSection(nameof(FamApiRoutingOptions)).Get<FamApiRoutingOptions>() ?? new FamApiRoutingOptions());
            services.AddHostedServiceTelemetryWrapper();
            services.AddTransient<Data.Contracts.IApiService, ApiService>();
            services.AddTransient<Data.Contracts.IApiDataProcessorService, ApiDataProcessorService>();
            //services.AddHostedService<StaticContentReloadBackgroundService>();
            //services.AddSubscriptionBackgroundService(configuration);

            const string AppSettingsPolicies = "Policies";
            var policyOptions = configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();

            services
                .AddPolicies(policyRegistry, nameof(FamApiRoutingOptions), policyOptions)
                .AddHttpClient<IRoutingService, RoutingService, FamApiRoutingOptions>(configuration, nameof(FamApiRoutingOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services.AddSingleton(configuration.GetSection(NotifyOptionsAppSettings).Get<NotifyOptions>() ?? new NotifyOptions());

            var notifyPolicyOptions = configuration.GetSection($"{NotifyOptionsAppSettings}:Policies").Get<PolicyOptions>() ?? new PolicyOptions();
            services.AddPolicies(policyRegistry, nameof(NotifyClientProxy), notifyPolicyOptions);
            services.AddHttpClient<INotifyClientProxy, NotifyClientProxy>()
            .AddPolicyHandlerFromRegistry($"{nameof(NotifyClientProxy)}_{nameof(notifyPolicyOptions.HttpRetry)}")
            .AddPolicyHandlerFromRegistry($"{nameof(NotifyClientProxy)}_{nameof(notifyPolicyOptions.HttpCircuitBreaker)}");

            services.AddMvc(config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
    }
}