using AutoMapper;
using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.HostedServices;
using DFC.App.ContactUs.HttpClientPolicies;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.AreaRoutingService;
using DFC.App.ContactUs.Services.CacheContentService;
using DFC.App.ContactUs.Services.EmailService;
using DFC.App.ContactUs.Services.EmailTemplateService;
using DFC.App.ContactUs.Services.Services.EmailService;
using DFC.Compui.Cosmos;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Subscriptions.Pkg.Webhook.Extensions;
using DFC.Compui.Subscriptions.Pkg.Webhook.Services;
using DFC.Compui.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;
using SendGrid.Helpers.Reliability;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CosmosDbContentPagesConfigAppSettings = "Configuration:CosmosDbConnections:ContentPages";
        private const string CosmosDbSessionStateConfigAppSettings = "Configuration:CosmosDbConnections:SessionState";
        private const string SendGridAppSettings = "Configuration:SendGrid";

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

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
            var cosmosDbConnectionContentPages = configuration.GetSection(CosmosDbContentPagesConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosDbConnectionSessionState = configuration.GetSection(CosmosDbSessionStateConfigAppSettings).Get<CosmosDbConnection>();
            services.AddContentPageServices<ContentPageModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
            services.AddDocumentServices<EmailModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
            services.AddDocumentServices<ContentPageModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
            services.AddSessionStateServices<SessionDataModel>(cosmosDbConnectionSessionState, env.IsDevelopment());

            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddSingleton<IContentCacheService>(new ContentCacheService());
            services.AddSingleton(new ServiceOpenDetailModel());
            services.AddSingleton<ValidationHtmlAttributeProvider, CustomValidationHtmlAttributeProvider>();
            services.AddSingleton(ConfigureSendGridClient());
            services.AddTransient<IMergeEmailContentService, MergeEmailContentService>();
            services.AddTransient<ISendGridEmailService<ContactUsEmailRequestModel>, SendGridEmailService<ContactUsEmailRequestModel>>();
            services.AddTransient<ITemplateService, TemplateService>();

            services.AddWebhookSupportNoChildren<EmailModel>();
            services.AddTransient<IEmailCacheReloadService, EmailCacheReloadService>();

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSingleton(configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddSingleton(configuration.GetSection(nameof(ChatOptions)).Get<ChatOptions>() ?? new ChatOptions());
            services.AddSingleton(configuration.GetSection(nameof(FamApiRoutingOptions)).Get<FamApiRoutingOptions>() ?? new FamApiRoutingOptions());
            services.AddSingleton(configuration.GetSection(nameof(WebhookSettings)).Get<WebhookSettings>() ?? new WebhookSettings());
            services.AddHostedServiceTelemetryWrapper();
            services.AddHostedService<CacheReloadBackgroundService>();
            services.AddSubscriptionBackgroundService(configuration);

            const string AppSettingsPolicies = "Policies";
            var policyOptions = configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();

            services
               .AddPolicies(policyRegistry, nameof(CmsApiClientOptions), policyOptions)
               .AddHttpClient<IEmailCacheReloadService, EmailCacheReloadService, CmsApiClientOptions>(configuration, nameof(CmsApiClientOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker))
               .AddHttpClient<ICmsApiService, CmsApiService, CmsApiClientOptions>(configuration, nameof(CmsApiClientOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services
                .AddPolicies(policyRegistry, nameof(FamApiRoutingOptions), policyOptions)
                .AddHttpClient<IRoutingService, RoutingService, FamApiRoutingOptions>(configuration, nameof(FamApiRoutingOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services.AddMvc(config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public ISendGridClient ConfigureSendGridClient()
        {
            var sendGridSettings = configuration.GetSection(SendGridAppSettings).Get<SendGridSettings>() ?? new SendGridSettings();
            var sendGridClient = new SendGridClient(new SendGridClientOptions
            {
                ApiKey = sendGridSettings.ApiKey,
                ReliabilitySettings = new ReliabilitySettings(
                    sendGridSettings.DefaultNumberOfRetries,
                    TimeSpan.FromSeconds(sendGridSettings.DefaultMinimumBackOff),
                    TimeSpan.FromSeconds(sendGridSettings.DefaultMaximumBackOff),
                    TimeSpan.FromSeconds(sendGridSettings.DeltaBackOff)),
            });

            return sendGridClient;
        }
    }
}