using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService<ContentPageModel> contentPageModelEventMessageService;
        private readonly IEventMessageService<EmailModel> emailModelEventMessageService;
        private readonly IContentPageService<ContentPageModel> contentPageService;
        private readonly IEmailCacheReloadService emailReloadService;
        //private readonly ISharedContentCacheReloadService contentPageModelReloadService;
        private readonly IContentCacheService contentCacheService;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            AutoMapper.IMapper mapper,
            IEventMessageService<ContentPageModel> contentPageModelEventMessageService,
            IContentPageService<ContentPageModel> contentPageService,
            IContentCacheService contentCacheService,
            IEmailCacheReloadService emailReloadService)
        //ISharedContentCacheReloadService contentPageModelReloadService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.contentPageModelEventMessageService = contentPageModelEventMessageService;
            this.contentPageService = contentPageService;
            this.contentCacheService = contentCacheService;
            this.emailReloadService = emailReloadService;
            //this.contentPageModelReloadService = contentPageModelReloadService;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!Guid.TryParse(url.Segments.LastOrDefault(), out Guid id))
            {
                throw new InvalidDataException($"Invalid id '{id}' received for Event Id: {eventId}");
            }

            var contentType = url.Segments[url.Segments.Length - 2].Trim('/').ToLowerInvariant();

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    switch (contentType.ToLowerInvariant())
                    {
                        case "email":
                            return await emailModelEventMessageService.DeleteAsync(id).ConfigureAwait(false);
                        case "static-page":
                            return await contentPageModelEventMessageService.DeleteAsync(id).ConfigureAwait(false);
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.Delete)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.OK;
                    }

                case WebhookCacheOperation.CreateOrUpdate:
                    switch (contentType.ToLowerInvariant())
                    {
                        case "email":
                            await emailReloadService.ReloadCacheItem(url).ConfigureAwait(false);
                            return HttpStatusCode.OK;
                        case "static-page":
                            //await contentPageModelReloadService.ReloadCacheItem(url).ConfigureAwait(false);
                            return HttpStatusCode.OK;
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.CreateOrUpdate)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.OK;
                    }


                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId, string contentType)
        {
            var result = await contentPageModelEventMessageService.DeleteAsync(contentId).ConfigureAwait(false);

            if (result == HttpStatusCode.OK)
            {
                contentCacheService.Remove(contentId);
            }

            return result;
        }

        public async Task<HttpStatusCode> DeleteContentItemAsync(Guid contentItemId, string contentType)
        {
            var contentIds = contentCacheService.GetContentIdsContainingContentItemId(contentItemId);

            if (!contentIds.Any())
            {
                return HttpStatusCode.NoContent;
            }

            foreach (var contentId in contentIds)
            {
                var contentPageModel = await contentPageService.GetByIdAsync(contentId).ConfigureAwait(false);

                if (contentPageModel != null)
                {
                    var contentItemModel = contentPageModel.ContentItems.FirstOrDefault(f => f.ItemId == contentItemId);

                    if (contentItemModel != null)
                    {
                        contentPageModel.ContentItems!.Remove(contentItemModel);

                        var result = await contentPageModelEventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

                        if (result == HttpStatusCode.OK)
                        {
                            contentCacheService.RemoveContentItem(contentId, contentItemId);
                        }
                    }
                }
            }

            return HttpStatusCode.OK;
        }

        public bool TryValidateModel(ContentPageModel contentPageModel)
        {
            _ = contentPageModel ?? throw new ArgumentNullException(nameof(contentPageModel));

            var validationContext = new ValidationContext(contentPageModel, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(contentPageModel, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {contentPageModel.CanonicalName} - {contentPageModel.Url}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
