using DFC.App.ContactUs.Data.Contracts;
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
    public class ConfigurationSetBackgroundService : BackgroundService
    {
        private readonly ILogger<ConfigurationSetBackgroundService> logger;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IConfigurationSetReloadService configurationSetReloadService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public ConfigurationSetBackgroundService(ILogger<ConfigurationSetBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, IConfigurationSetReloadService configurationSetReloadService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.logger = logger;
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.hostedServiceTelemetryWrapper = hostedServiceTelemetryWrapper;
            this.configurationSetReloadService = configurationSetReloadService;
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
                    logger.LogInformation($"CMS Api Client Base Address is null, skipping Configuration set Reload");
                }

                logger.LogInformation($"Executing Telemetry wrapper with service {nameof(configurationSetReloadService)}");

                var configurationSetReloadServiceTask = hostedServiceTelemetryWrapper.Execute(async () => await configurationSetReloadService.Reload(stoppingToken).ConfigureAwait(false), nameof(ConfigurationSetBackgroundService));
                await configurationSetReloadServiceTask.ConfigureAwait(false);

                //Caters for errors in the telemetry wrapper
                if (!configurationSetReloadServiceTask.IsCompletedSuccessfully)
                {
                    logger.LogInformation($"An error occurred in the {nameof(hostedServiceTelemetryWrapper)}");

                    if (configurationSetReloadServiceTask.Exception != null)
                    {
                        logger.LogError(configurationSetReloadServiceTask.Exception.ToString());
                        throw configurationSetReloadServiceTask.Exception;
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
