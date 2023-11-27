using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models.CmsApiModels;
using DFC.App.ContactUs.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;

using Microsoft.Extensions.Logging;

namespace DFC.App.ContactUs.Services
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly ICmsApiService cmsApiService;
        private readonly IDocumentService<SharedContentItemModel> sharedContentItemDocumentService;
        private readonly Guid sharedContentId;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            AutoMapper.IMapper mapper,
            CmsApiClientOptions cmsApiClientOptions,
            ICmsApiService cmsApiService,
            IDocumentService<SharedContentItemModel> sharedContentItemDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cmsApiService = cmsApiService;
            this.sharedContentItemDocumentService = sharedContentItemDocumentService;
            sharedContentId = Guid.Parse(cmsApiClientOptions?.ContentIds);
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            if (sharedContentId != contentId)
            {
                logger.LogInformation($"Event Id: {eventId}, is not a shared content item we are subscribed to, so no content has been processed");
                return HttpStatusCode.OK;
            }

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await DeleteContentAsync(contentId);

                case WebhookCacheOperation.CreateOrUpdate:
                    if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}");
                    }

                    return await ProcessContentAsync(url);

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> ProcessContentAsync(Uri url)
        {
            var apiDataModel = await cmsApiService.GetItemAsync<SharedContentItemApiDataModel>(url);
            var sharedContentItemModel = mapper.Map<SharedContentItemModel>(apiDataModel);

            if (sharedContentItemModel == null)
            {
                return HttpStatusCode.NoContent;
            }

            if (!TryValidateModel(sharedContentItemModel))
            {
                return HttpStatusCode.BadRequest;
            }

            var contentResult = await sharedContentItemDocumentService.UpsertAsync(sharedContentItemModel);

            return contentResult;
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            var result = await sharedContentItemDocumentService.DeleteAsync(contentId);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }

        public bool TryValidateModel(SharedContentItemModel? sharedContentItemModel)
        {
            _ = sharedContentItemModel ?? throw new ArgumentNullException(nameof(sharedContentItemModel));

            var validationContext = new ValidationContext(sharedContentItemModel, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(sharedContentItemModel, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {sharedContentItemModel.Title} - {sharedContentItemModel.Url}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
