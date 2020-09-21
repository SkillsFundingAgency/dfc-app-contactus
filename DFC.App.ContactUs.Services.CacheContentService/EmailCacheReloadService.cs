using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
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

        public EmailCacheReloadService(ILogger<EmailCacheReloadService> logger, AutoMapper.IMapper mapper, IDocumentService<EmailModel> emailDocumentService, ICmsApiService cmsApiService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.emailDocumentService = emailDocumentService;
            this.cmsApiService = cmsApiService;
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

                await ReloadEmailTemplates(stoppingToken).ConfigureAwait(false);

                logger.LogInformation("Reload All email cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in email cache reload");
                throw;
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
