using DFC.App.ContactUs.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService
{
    public class ContentApiService<TApiResponseModel> : IContentApiService<TApiResponseModel>
        where TApiResponseModel : class
    {
        private readonly IApiDataProcessorService apiDataProcessorService;
        private readonly HttpClient httpClient;

        public ContentApiService(IApiDataProcessorService apiDataProcessorService, HttpClient httpClient)
        {
            this.apiDataProcessorService = apiDataProcessorService ?? throw new ArgumentNullException(nameof(apiDataProcessorService));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<TApiResponseModel>> GetAll(string contentType)
        {
            return await apiDataProcessorService.GetAsync<IEnumerable<TApiResponseModel>>(httpClient, contentType).ConfigureAwait(false) ?? throw new InvalidOperationException($"{nameof(GetAll)} returned null for content type {contentType}");
        }

        public async Task<TApiResponseModel> GetById(string contentType, string id)
        {
            return await apiDataProcessorService.GetAsync<TApiResponseModel>(httpClient, contentType, id).ConfigureAwait(false) ?? throw new InvalidOperationException($"{nameof(GetById)} returned null for request {contentType} {id}");
        }

        public async Task<TApiResponseModel> GetById(Uri uri)
        {
            return await apiDataProcessorService.GetAsync<TApiResponseModel>(httpClient, uri).ConfigureAwait(false) ?? throw new InvalidOperationException($"{nameof(GetById)} returned null for request {uri}");
        }
    }
}
