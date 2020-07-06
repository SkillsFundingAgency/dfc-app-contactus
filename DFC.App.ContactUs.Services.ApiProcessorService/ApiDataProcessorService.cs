using DFC.App.ContactUs.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.ApiProcessorService
{
    public class ApiDataProcessorService : IApiDataProcessorService
    {
        private readonly IApiService apiService;

        public ApiDataProcessorService(IApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, Uri url)
            where TApiModel : class
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            var response = await apiService.GetAsync(httpClient, url, MediaTypeNames.Application.Json).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(response))
            {
                return JsonConvert.DeserializeObject<TApiModel>(response);
            }

            return default;
        }

        public async Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType, string id)
           where TApiModel : class
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            var response = await apiService.GetAsync(httpClient, contentType, id, MediaTypeNames.Application.Json).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(response))
            {
                return JsonConvert.DeserializeObject<TApiModel>(response);
            }

            return default;
        }

        public async Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType)
           where TApiModel : class
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            var response = await apiService.GetAsync(httpClient, contentType, MediaTypeNames.Application.Json).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(response))
            {
                return JsonConvert.DeserializeObject<TApiModel>(response);
            }

            return default;
        }
    }
}
