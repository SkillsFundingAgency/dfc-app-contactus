using DFC.App.ContactUs.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.PageService.Contracts
{
    public interface IContentPageService<TModel>
        where TModel : class, IServiceDataModel
    {
        Task<bool> PingAsync();

        Task<IEnumerable<TModel>?> GetAllAsync();

        Task<TModel?> GetByIdAsync(Guid documentId);

        Task<TModel?> GetByNameAsync(string? canonicalName);

        Task<TModel?> GetByAlternativeNameAsync(string? alternativeName);

        Task<HttpStatusCode> UpsertAsync(TModel? model);

        Task<bool> DeleteAsync(Guid documentId);
    }
}