using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService
{
    public class CmsApiService : ICmsApiService
    {
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IApiDataProcessorService apiDataProcessorService;
        private readonly HttpClient httpClient;

        public CmsApiService(CmsApiClientOptions cmsApiClientOptions, IApiDataProcessorService apiDataProcessorService, HttpClient httpClient)
        {
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.apiDataProcessorService = apiDataProcessorService;
            this.httpClient = httpClient;
        }

        public async Task<IList<TModel>?> GetAll<TModel>()
            where TModel : class
        {
            var url = new Uri($"{cmsApiClientOptions.BaseAddress}{cmsApiClientOptions.SummaryEndpoint}", UriKind.Absolute);

            return await apiDataProcessorService.GetAsync<IList<TModel>>(httpClient, url).ConfigureAwait(false);
        }

        public async Task<TModel?> GetContentItemAsync<TModel>(Uri url)
            where TModel : class
        {
            return await apiDataProcessorService.GetAsync<TModel>(httpClient, url).ConfigureAwait(false);
        }
    }
}
