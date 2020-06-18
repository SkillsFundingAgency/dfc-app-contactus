using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class EmailReloadService : IEmailReloadService
    {
        private readonly ICmsApiService cmsApiService;
        private readonly IContentApiService<EmailApiDataModel> contentApiService;
        private readonly ILogger<CacheReloadService> logger;

        public EmailReloadService(ICmsApiService cmsApiService, IContentApiService<EmailApiDataModel> contentApiService, ILogger<CacheReloadService> logger)
        {
            this.cmsApiService = cmsApiService;
            this.contentApiService = contentApiService;
            this.logger = logger;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload email cache started");
                await GetEmails();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task GetEmails()
        {
            var emails = await contentApiService.GetAll("email").ConfigureAwait(false);

            var emailKeys = EmailKeyHelper.GetEmailKeys();

            foreach (var key in emailKeys)
            {
                var emailToCache = emails.FirstOrDefault(x => x.Url!.ToString().Contains(key.ToString(), StringComparison.OrdinalIgnoreCase));

                if (emailToCache == null)
                {
                    //Throw an exception if an essential e-mail hasn't been found?
                }

                //Add the e-mail to cache
            }
        }
    }
}
