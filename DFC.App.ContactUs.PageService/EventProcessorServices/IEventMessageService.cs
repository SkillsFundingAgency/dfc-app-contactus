using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.PageService.EventProcessorServices
{
    public interface IEventMessageService
    {
        Task<IList<ContentPageModel>?> GetAllCachedCanonicalNamesAsync();

        Task<HttpStatusCode> CreateAsync(ContentPageModel? upsertContentPageModel);

        Task<HttpStatusCode> UpdateAsync(ContentPageModel? upsertContentPageModel);

        Task<HttpStatusCode> DeleteAsync(Guid documentId);
    }
}