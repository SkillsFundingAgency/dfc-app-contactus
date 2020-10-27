using DFC.App.ContactUs.Data.Common;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class ConfigurationSetReloadService : IConfigurationSetReloadService
    {
        private readonly ILogger<ConfigurationSetReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IDocumentService<ConfigurationSetModel> configurationSetDocumentService;
        private readonly ICmsApiService cmsApiService;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IContentTypeMappingService contentTypeMappingService;

        public ConfigurationSetReloadService(
            ILogger<ConfigurationSetReloadService> logger,
            AutoMapper.IMapper mapper,
            IDocumentService<ConfigurationSetModel> configurationSetDocumentService,
            ICmsApiService cmsApiService,
            CmsApiClientOptions cmsApiClientOptions,
            IContentTypeMappingService contentTypeMappingService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.configurationSetDocumentService = configurationSetDocumentService;
            this.cmsApiService = cmsApiService;
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.contentTypeMappingService = contentTypeMappingService;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload configuration set started");

                contentTypeMappingService.AddMapping(Constants.ContentTypeForConfigurationItem, typeof(ConfigurationItemApiDataModel));

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload configuration set cancelled");

                    return;
                }

                await ReloadConfigurationSetTemplate(stoppingToken).ConfigureAwait(false);

                logger.LogInformation("Reload All configuration set completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in configuration set reload");
                throw;
            }
        }

        private async Task ReloadConfigurationSetTemplate(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning("Reload configuration set cancelled");

                return;
            }

            var key = ConfigurationSetKeyHelper.ConfigurationSetKey;

            var url = new Uri($"{cmsApiClientOptions.BaseAddress}/{Constants.ContentTypeForConfigurationSet}/{key}", UriKind.Absolute);
            var apiDataModel = await cmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(url).ConfigureAwait(false);

            if (apiDataModel == null)
            {
                logger.LogError($"Configuration set: {key} not found in API response");
                return;
            }

            if (!apiDataModel.ContentItems.Any())
            {
                logger.LogError($"Configuration set: {key} contains no items");
                return;
            }

            var configurationSetModel = mapper.Map<ConfigurationSetModel>(apiDataModel);

            await configurationSetDocumentService.UpsertAsync(configurationSetModel).ConfigureAwait(false);
        }
    }
}
