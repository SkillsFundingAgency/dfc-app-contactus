using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.PageService.EventProcessorServices
{
    public class EventMessageService : IEventMessageService
    {
        private readonly ILogger<EventMessageService> logger;
        private readonly IContentPageService contentPageService;

        public EventMessageService(ILogger<EventMessageService> logger, IContentPageService contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
        }

        public async Task<IList<ContentPageModel>?> GetAllCachedCanonicalNamesAsync()
        {
            var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            return contentPageModels.ToList();
        }

        public async Task<HttpStatusCode> CreateAsync(ContentPageModel? upsertContentPageModel)
        {
            if (upsertContentPageModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(CreateAsync)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

            return response;
        }

        public async Task<HttpStatusCode> UpdateAsync(ContentPageModel? upsertContentPageModel)
        {
            if (upsertContentPageModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (upsertContentPageModel.Version.Equals(existingDocument.Version))
            {
                return HttpStatusCode.AlreadyReported;
            }

            upsertContentPageModel.Etag = existingDocument.Etag;

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(UpdateAsync)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

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
