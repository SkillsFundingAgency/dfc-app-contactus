using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Services.PageService.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.PageService
{
    public class ContentPageService<TModel> : IContentPageService<TModel>
            where TModel : class, IServiceDataModel
    {
        private readonly ICosmosRepository<TModel> repository;

        public ContentPageService(ICosmosRepository<TModel> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>?> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<TModel?> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<TModel?> GetByNameAsync(string? canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<TModel?> GetByAlternativeNameAsync(string? alternativeName)
        {
            if (string.IsNullOrWhiteSpace(alternativeName))
            {
                throw new ArgumentNullException(nameof(alternativeName));
            }

            return await repository.GetAsync(d => d.AlternativeNames!.Contains(alternativeName.ToLowerInvariant())).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> UpsertAsync(TModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return await repository.UpsertAsync(model).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}