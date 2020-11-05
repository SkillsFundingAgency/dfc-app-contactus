﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.Compui.Telemetry.HostedService;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    [ExcludeFromCodeCoverage]
    public class EmailCacheReloadBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailCacheReloadBackgroundService> logger;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IEmailCacheReloadService emailCacheReloadService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public EmailCacheReloadBackgroundService(ILogger<EmailCacheReloadBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, IEmailCacheReloadService emailCacheReloadService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (cmsApiClientOptions.BaseAddress == null)
                {
                    logger.LogInformation($"CMS Api Client Base Address is null, skipping Cache Reload");
                }

                logger.LogInformation($"Executing Telemetry wrapper with service {nameof(emailCacheReloadService)}");

                var emailCacheReloadServiceTask = hostedServiceTelemetryWrapper.Execute(async () => await emailCacheReloadService.Reload(stoppingToken).ConfigureAwait(false), nameof(EmailCacheReloadBackgroundService));
                await emailCacheReloadServiceTask.ConfigureAwait(false);

                //Caters for errors in the telemetry wrapper
                if (!emailCacheReloadServiceTask.IsCompletedSuccessfully)
                {
                    logger.LogInformation($"An error occurred in the {nameof(hostedServiceTelemetryWrapper)}");

                    if (emailCacheReloadServiceTask.Exception != null)
                    {
                        logger.LogError(emailCacheReloadServiceTask.Exception.ToString());
                        throw emailCacheReloadServiceTask.Exception;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
