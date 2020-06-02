using AutoMapper;
using CorrelationId;
using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.ClientHandlers;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Filters;
using DFC.App.ContactUs.HostedServices;
using DFC.App.ContactUs.HttpClientPolicies;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Repository.CosmosDb;
using DFC.App.ContactUs.Services.ApiProcessorService;
using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService;
using DFC.App.ContactUs.Services.AreaRoutingService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies;
using DFC.App.ContactUs.Services.CmsApiProcessorService;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.HttpClientPolicies;
using DFC.App.ContactUs.Services.EmailService;
using DFC.App.ContactUs.Services.EmailService.Contracts;
using DFC.App.ContactUs.Services.EmailService.Models;
using DFC.App.ContactUs.Services.EmailTemplateService;
using DFC.App.ContactUs.Services.EmailTemplateService.Contracts;
using DFC.App.ContactUs.Services.EventProcessorService;
using DFC.App.ContactUs.Services.EventProcessorService.Contracts;
using DFC.App.ContactUs.Services.PageService;
using DFC.App.ContactUs.Services.PageService.Contracts;
using DFC.App.ContactUs.Services.Services.EmailService;
using DFC.Logger.AppInsights.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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
        private const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:ContentPages";
        private const string SendGridAppSettings = "Configuration:SendGrid";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = "DssCorrelationId",
                UseGuidForCorrelationId = true,
                UpdateTraceIdentifier = false,
            });

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
            app.UseCookiePolicy();
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(cosmosDbConnection!.EndpointUrl, cosmosDbConnection!.AccessKey);
            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddCorrelationId();
            services.AddSingleton(new ServiceOpenDetailModel());
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<ContentPageModel>, CosmosRepository<ContentPageModel>>();
            services.AddSingleton<ValidationHtmlAttributeProvider, CustomValidationHtmlAttributeProvider>();
            services.AddSingleton(ConfigureSendGridClient());
            services.AddTransient<IMergeEmailContentService, MergeEmailContentService>();
            services.AddTransient<ISendGridEmailService<ContactUsEmailRequestModel>, SendGridEmailService<ContactUsEmailRequestModel>>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<IContentPageService<ContentPageModel>, ContentPageService<ContentPageModel>>();
            services.AddTransient<IEventMessageService<ContentPageModel>, EventMessageService<ContentPageModel>>();
            services.AddTransient<ICacheReloadService, CacheReloadService>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<IApiDataProcessorService, ApiDataProcessorService>();
            services.AddTransient<CorrelationIdDelegatingHandler>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddDFCLogging(configuration["ApplicationInsights:InstrumentationKey"]);
            services.AddSingleton(configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddSingleton(configuration.GetSection(nameof(ChatOptions)).Get<ChatOptions>() ?? new ChatOptions());
            services.AddSingleton(configuration.GetSection(nameof(FamApiRoutingOptions)).Get<FamApiRoutingOptions>() ?? new FamApiRoutingOptions());
            services.AddHostedService<CacheReloadBackgroundService>();

            const string AppSettingsPolicies = "Policies";
            var policyOptions = configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();

            services
                .AddPolicies(policyRegistry, nameof(CmsApiClientOptions), policyOptions)
                .AddHttpClient<ICmsApiService, CmsApiService, CmsApiClientOptions>(configuration, nameof(CmsApiClientOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services
                .AddPolicies(policyRegistry, nameof(FamApiRoutingOptions), policyOptions)
                .AddHttpClient<IRoutingService, RoutingService, FamApiRoutingOptions>(configuration, nameof(FamApiRoutingOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services.AddMvc(config =>
                {
                    config.Filters.Add<LoggingAsynchActionFilter>();
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