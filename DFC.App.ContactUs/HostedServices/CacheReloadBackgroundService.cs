using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Telemetry.HostedService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    public class CacheReloadBackgroundService : BackgroundService
    {
        private readonly ILogger<CacheReloadBackgroundService> logger;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IEmailCacheReloadService emailCacheReloadService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public CacheReloadBackgroundService(ILogger<CacheReloadBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, /*ISharedContentCacheReloadService sharedContentCacheReloadService,*/ IEmailCacheReloadService emailCacheReloadService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.logger = logger;
            this.cmsApiClientOptions = cmsApiClientOptions;
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
            if (cmsApiClientOptions.BaseAddress != null)
            {
                logger.LogInformation("Cache reload executing");
                emailCacheReloadService.Reload(stoppingToken).ConfigureAwait(false).GetAwaiter().GetResult();

                //if (!task.IsCompletedSuccessfully)
                //{
                //    logger.LogInformation("Cache reload didn't complete successfully");
                //    if (task.Exception != null)
                //    {
                //        logger.LogError(task.Exception.ToString());
                //        throw task.Exception;
                //    }
                //}

                //return task;
            }

            return Task.CompletedTask;
        }
    }
}
