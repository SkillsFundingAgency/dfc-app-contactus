using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Services.EventProcessorService.Contracts;
using DFC.App.ContactUs.Services.PageService.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EventProcessorService
{
    public class EventMessageService<TModel> : IEventMessageService<TModel>
           where TModel : class, IServiceDataModel
    {
        private readonly ILogger<EventMessageService<TModel>> logger;
        private readonly IContentPageService<TModel> contentPageService;

        public EventMessageService(ILogger<EventMessageService<TModel>> logger, IContentPageService<TModel> contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
        }

        public async Task<IList<TModel>?> GetAllCachedCanonicalNamesAsync()
        {
            var serviceDataModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            return serviceDataModels.ToList();
        }

        public async Task<HttpStatusCode> CreateAsync(TModel? upsertServiceDataModel)
        {
            if (upsertServiceDataModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertServiceDataModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var response = await contentPageService.UpsertAsync(upsertServiceDataModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(CreateAsync)} has upserted content for: {upsertServiceDataModel.CanonicalName} with response code {response}");

            return response;
        }

        public async Task<HttpStatusCode> UpdateAsync(TModel? upsertServiceDataModel)
        {
            if (upsertServiceDataModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertServiceDataModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (upsertServiceDataModel.Version.Equals(existingDocument.Version))
            {
                return HttpStatusCode.AlreadyReported;
            }

            upsertServiceDataModel.Etag = existingDocument.Etag;

            var response = await contentPageService.UpsertAsync(upsertServiceDataModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(UpdateAsync)} has upserted content for: {upsertServiceDataModel.CanonicalName} with response code {response}");

            return response;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid documentId)
        {
            var isDeleted = await contentPageService.DeleteAsync(documentId).ConfigureAwait(false);

            if (isDeleted)
            {
                logger.LogInformation($"{nameof(DeleteAsync)} has deleted content for document Id: {documentId}");
                return HttpStatusCode.OK;
            }
            else
            {
                logger.LogWarning($"{nameof(DeleteAsync)} has returned no content for: {documentId}");
                return HttpStatusCode.NotFound;
            }
        }
    }
}
