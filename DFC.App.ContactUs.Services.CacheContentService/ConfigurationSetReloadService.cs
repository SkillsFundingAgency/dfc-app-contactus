using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task ReloadConfigurationSetTemplate(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning("Reload configuration set cancelled");

                return;
            }

            var key = Guid.Parse("87a3aa46-472c-47a1-96c1-1a009b5953eb");
            const string contentTypeForSet = "IanTestListContentType";
            const string contentTypeForItem = "IanTestContentType";

            contentTypeMappingService.AddMapping(contentTypeForItem, typeof(ConfigurationItemApiDataModel));

            var url = new Uri($"{cmsApiClientOptions.BaseAddress}/{contentTypeForSet}/{key}", UriKind.Absolute);
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

            var configurationSetModel = new ConfigurationSetModel();
            var dict = apiDataModel.ContentItems.Select(s => s as ConfigurationItemApiDataModel).ToDictionary(k => k.Title, v => v.Value);
            if (dict.ContainsKey("Telephone number"))
            {
                configurationSetModel.PhoneNumber = dict["Telephone number"] ?? string.Empty;
            }

            if (dict.ContainsKey("Lines open text"))
            {
                configurationSetModel.LinesOpenText = dict["Lines open text"] ?? string.Empty;
            }

            if (dict.ContainsKey("Open time from"))
            {
                if (Enum.TryParse(dict["Open time from"], out TimeSpan openTimeFrom))
                {
                    configurationSetModel.OpenTimeFrom = openTimeFrom;
                }
            }

            if (dict.ContainsKey("Open time to"))
            {
                if (Enum.TryParse(dict["Open time to"], out TimeSpan openTimeTo))
                {
                    configurationSetModel.OpenTimeTo = openTimeTo;
                }
            }

            if (dict.ContainsKey("Week day from"))
            {
                if (Enum.TryParse(dict["Week day from"], out DayOfWeek weekDayFrom))
                {
                    configurationSetModel.WeekdayFrom = weekDayFrom;
                }
            }

            if (dict.ContainsKey("Week day to"))
            {
                if (Enum.TryParse(dict["Week day to"], out DayOfWeek weekDayTo))
                {
                    configurationSetModel.WeekdayTo = weekDayTo;
                }
            }

            await configurationSetDocumentService.UpsertAsync(configurationSetModel).ConfigureAwait(false);
        }
    }
}
