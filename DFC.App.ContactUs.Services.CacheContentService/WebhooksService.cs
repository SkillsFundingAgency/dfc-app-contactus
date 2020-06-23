using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly IEventMessageService<EmailModel> emailModelEventMessageService;
        private readonly IEmailCacheReloadService emailReloadService;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            IEventMessageService<EmailModel> emailModelEventMessageService,
            IEmailCacheReloadService emailReloadService)
        {
            this.logger = logger;
            this.emailModelEventMessageService = emailModelEventMessageService;
            this.emailReloadService = emailReloadService;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Uri url)
        {
            Guid id = GetIdFromUrl(eventId, url);

            if (url.Segments.Length < 3)
            {
                throw new InvalidDataException($"URI: {url} doesn't contian enough segments for a Content Type and Id");
            }

            var contentType = url.Segments[url.Segments.Length - 2].Trim('/').ToUpperInvariant();

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    switch (contentType.ToUpperInvariant())
                    {
                        case "EMAIL":
                            return await emailModelEventMessageService.DeleteAsync(id).ConfigureAwait(false);
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.Delete)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.NotFound;
                    }

                case WebhookCacheOperation.CreateOrUpdate:
                    switch (contentType.ToUpperInvariant())
                    {
                        case "EMAIL":
                            await emailReloadService.ReloadCacheItem(url).ConfigureAwait(false);
                            return HttpStatusCode.Created;
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.CreateOrUpdate)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.OK;
                    }

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        private static Guid GetIdFromUrl(Guid eventId, Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!Guid.TryParse(url.Segments.LastOrDefault(), out Guid id))
            {
                throw new InvalidDataException($"Invalid id '{id}' received for Event Id: {eventId}");
            }

            return id;
        }
    }
}
