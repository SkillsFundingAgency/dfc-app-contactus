﻿using DFC.App.ContactUs.Data.Contracts;
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
        private readonly ICacheReloadService cacheReloadService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public CacheReloadBackgroundService(ILogger<CacheReloadBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, ICacheReloadService cacheReloadService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.logger = logger;
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.cacheReloadService = cacheReloadService;
            this.hostedServiceTelemetryWrapper = hostedServiceTelemetryWrapper;
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
                var cacheReloadServiceTask = hostedServiceTelemetryWrapper.Execute(() => cacheReloadService.Reload(stoppingToken), nameof(CacheReloadBackgroundService));

                return cacheReloadServiceTask;
            }

            return Task.CompletedTask;
        }
    }
}
