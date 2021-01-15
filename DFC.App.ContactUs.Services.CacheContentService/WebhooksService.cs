using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
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
        private readonly ICmsApiService cmsApiService;
        private readonly IDocumentService<ConfigurationSetModel> configurationSetDocumentService;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            AutoMapper.IMapper mapper,
            ICmsApiService cmsApiService,
            IDocumentService<ConfigurationSetModel> configurationSetDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cmsApiService = cmsApiService;
            this.configurationSetDocumentService = configurationSetDocumentService;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await DeleteConfigurationSetAsync(contentId).ConfigureAwait(false);

                case WebhookCacheOperation.CreateOrUpdate:
                    if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}");
                    }

                    return await ProcessConfigurationSetAsync(url).ConfigureAwait(false);

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> ProcessConfigurationSetAsync(Uri url)
        {
            var apiDataModel = await cmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(url).ConfigureAwait(false);
            var configurationSetModel = mapper.Map<ConfigurationSetModel>(apiDataModel);

            if (configurationSetModel == null)
            {
                return HttpStatusCode.NoContent;
            }

            if (!TryValidateModel(configurationSetModel))
            {
                return HttpStatusCode.BadRequest;
            }

            var contentResult = await configurationSetDocumentService.UpsertAsync(configurationSetModel).ConfigureAwait(false);

            return contentResult;
        }

        public async Task<HttpStatusCode> DeleteConfigurationSetAsync(Guid contentId)
        {
            var result = await configurationSetDocumentService.DeleteAsync(contentId).ConfigureAwait(false);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }

        public bool TryValidateModel<TModel>(TModel? model)
            where TModel : class, ICachedModel
        {
            _ = model ?? throw new ArgumentNullException(nameof(model));

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {model.Title} - {model.Url}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
