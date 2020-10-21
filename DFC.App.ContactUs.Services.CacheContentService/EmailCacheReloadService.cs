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
    public class EmailCacheReloadService : IEmailCacheReloadService
    {
        private readonly ILogger<EmailCacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IDocumentService<EmailModel> emailDocumentService;
        private readonly ICmsApiService cmsApiService;
        private readonly IContentTypeMappingService contentTypeMappingService;
        private readonly ServiceOpenDetailModel serviceOpenDetailModel;
        private readonly CmsApiClientOptions cmsApiClientOptions;

        public EmailCacheReloadService(ILogger<EmailCacheReloadService> logger, AutoMapper.IMapper mapper, IDocumentService<EmailModel> emailDocumentService, ICmsApiService cmsApiService, IContentTypeMappingService contentTypeMappingService, ServiceOpenDetailModel serviceOpenDetailModel, CmsApiClientOptions cmsApiClientOptions)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.emailDocumentService = emailDocumentService;
            this.cmsApiService = cmsApiService;
            this.contentTypeMappingService = contentTypeMappingService;
            this.serviceOpenDetailModel = serviceOpenDetailModel;
            this.cmsApiClientOptions = cmsApiClientOptions;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload All email cache started");

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload email cache cancelled");

                    return;
                }

                await ReloadServiceOpenDetail(stoppingToken).ConfigureAwait(false);

                await ReloadEmailTemplates(stoppingToken).ConfigureAwait(false);

                logger.LogInformation("Reload All email cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in email cache reload");
                throw;
            }
        }

        public async Task ReloadServiceOpenDetail(CancellationToken stoppingToken)
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

            var dict = apiDataModel.ContentItems.Select(s => s as ConfigurationItemApiDataModel).ToDictionary(k => k.Title, v => v.Value);
            if (dict.ContainsKey("Telephone number"))
            {
                serviceOpenDetailModel.PhoneNumber = dict["Telephone number"] ?? string.Empty;
            }

            if (dict.ContainsKey("Lines open text"))
            {
                serviceOpenDetailModel.LinesOpenText = dict["Lines open text"] ?? string.Empty;
            }

            if (dict.ContainsKey("Open time from"))
            {
                serviceOpenDetailModel.OpenTimeFrom = TimeSpan.Parse(dict["Open time from"] ?? "08:00:00");
            }

            if (dict.ContainsKey("Open time to"))
            {
                serviceOpenDetailModel.OpenTimeTo = TimeSpan.Parse(dict["Open time to"] ?? "22:00:00");
            }

            if (dict.ContainsKey("Week day from") && Enum.TryParse(dict["Week day from"] ?? $"{DayOfWeek.Monday}", out DayOfWeek weekDayFrom))
            {
                serviceOpenDetailModel.WeekdayFrom = weekDayFrom;
            }

            if (dict.ContainsKey("Week day to") && Enum.TryParse(dict["Week day to"] ?? $"{DayOfWeek.Sunday}", out DayOfWeek weekDayTo))
            {
                serviceOpenDetailModel.WeekdayTo = weekDayTo;
            }
        }

        public async Task ReloadEmailTemplates(CancellationToken stoppingToken)
        {
            var emailKeys = EmailKeyHelper.GetEmailKeys();

            foreach (var key in emailKeys)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload email cache cancelled");

                    return;
                }

                var apiDataModel = await cmsApiService.GetItemAsync<EmailApiDataModel>("email", key).ConfigureAwait(false);

                if (apiDataModel == null)
                {
                    logger.LogError($"Email Template: {key} not found in API response");
                }

                //Add the e-mail to cache
                var mappedEmail = mapper.Map<EmailModel>(apiDataModel);

                await emailDocumentService.UpsertAsync(mappedEmail).ConfigureAwait(false);
            }
        }
    }
}
