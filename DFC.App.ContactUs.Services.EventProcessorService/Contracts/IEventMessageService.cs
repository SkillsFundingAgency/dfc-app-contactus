using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EventProcessorService.Contracts
{
    public interface IEventMessageService<TModel>
        where TModel : class, IEventDataModel
    {
        Task<IList<TModel>?> GetAllCachedCanonicalNamesAsync();

        Task<HttpStatusCode> CreateAsync(TModel? upsertServiceDataModel);

        Task<HttpStatusCode> UpdateAsync(TModel? upsertServiceDataModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}