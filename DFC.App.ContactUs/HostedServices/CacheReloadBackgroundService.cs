using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Telemetry.HostedService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    public class CacheReloadBackgroundService : BackgroundService
    {
        private readonly ILogger<CacheReloadBackgroundService> logger;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly ISharedContentCacheReloadService sharedContentCacheReloadService;
        private readonly IEmailCacheReloadService emailCacheReloadService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public CacheReloadBackgroundService(ILogger<CacheReloadBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, /*ISharedContentCacheReloadService sharedContentCacheReloadService,*/ IEmailCacheReloadService emailCacheReloadService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.logger = logger;
            this.cmsApiClientOptions = cmsApiClientOptions;
            //this.sharedContentCacheReloadService = sharedContentCacheReloadService;
            this.hostedServiceTelemetryWrapper = hostedServiceTelemetryWrapper;
            this.emailCacheReloadService = emailCacheReloadService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Cache reload started");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Cache reload stopped");

            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (cmsApiClientOptions.BaseAddress == null)
            {
                logger.LogInformation($"CMS Api Client Base Address is null, skipping Cache Reload");
                return Task.CompletedTask;
            }

            //var contentPageModelCacheReloadServiceTask = hostedServiceTelemetryWrapper.Execute(() => sharedContentCacheReloadService.Reload(stoppingToken), nameof(CacheReloadBackgroundService));
            var emailCacheReloadServiceTask = hostedServiceTelemetryWrapper.Execute(() => emailCacheReloadService.Reload(stoppingToken), nameof(CacheReloadBackgroundService));

            return Task.CompletedTask;
        }
    }
}
