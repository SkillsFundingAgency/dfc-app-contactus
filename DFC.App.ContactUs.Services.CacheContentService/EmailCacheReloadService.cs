﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class EmailCacheReloadService : IEmailCacheReloadService
    {
        private readonly IDocumentService<EmailModel> emailDocumentService;
        private readonly IContentApiService<EmailApiDataModel> contentApiService;
        private readonly ILogger<EmailCacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;

        public EmailCacheReloadService(IContentApiService<EmailApiDataModel> contentApiService, ILogger<EmailCacheReloadService> logger, IDocumentService<EmailModel> emailDocumentService, AutoMapper.IMapper mapper)
        {
            this.emailDocumentService = emailDocumentService;
            this.contentApiService = contentApiService;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task ReloadCacheItem(Uri uri)
        {
            try
            {
                logger.LogInformation($"Reload email cache started - URI {uri}");

                await ReloadSingleTemplate(uri).ConfigureAwait(false);

                logger.LogInformation($"Reload email cache completed - URI {uri}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error in email cache reload - URI {uri}");
            }
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

                var apiEmail = await contentApiService.GetById("email", key.ToString()).ConfigureAwait(false);

                if (apiEmail == null)
                {
                    logger.LogError($"Email Template: {key} not found in API response");
                }

                //Add the e-mail to cache
                var mappedEmail = mapper.Map<EmailModel>(apiEmail);

                await emailDocumentService.UpsertAsync(mappedEmail).ConfigureAwait(false);
            }
        }

        private async Task ReloadSingleTemplate(Uri uri)
        {
            var email = await contentApiService.GetById(uri).ConfigureAwait(false);

            //Add the e-mail to cache
            var mappedEmail = mapper.Map<EmailModel>(email);

            await emailDocumentService.UpsertAsync(mappedEmail).ConfigureAwait(false);
        }
    }
}
