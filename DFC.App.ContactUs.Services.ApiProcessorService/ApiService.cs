using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.ApiProcessorService
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> logger;
        private readonly CmsApiClientOptions clientOptions;

        public ApiService(ILogger<ApiService> logger, CmsApiClientOptions clientOptions)
        {
            this.logger = logger;
            this.clientOptions = clientOptions;
        }

        public async Task<string?> GetAsync(HttpClient httpClient, string contentType, string acceptHeader)
        {
            return await GetAsync(httpClient, new Uri($"{this.clientOptions.BaseAddress}/{contentType}"), acceptHeader).ConfigureAwait(false);
        }

        public async Task<string?> GetAsync(HttpClient httpClient, Uri url, string acceptHeader)
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            logger.LogInformation($"Loading data from {url}");

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptHeader));
            request.Headers.Add("Ocp-Apim-Subscription-Key", clientOptions.ApiKey);

            try
            {
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                string? responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"Failed to get {acceptHeader} data from {url}, received error : '{responseString}', Returning empty content.");
                    responseString = null;
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    logger.LogInformation($"Status - {response.StatusCode} with response '{responseString}' received from {url}, Returning empty content.");
                    responseString = null;
                }

                return responseString;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error received getting {acceptHeader} data '{ex.InnerException?.Message}'. Received from {url}, Returning empty content.");
                return null;
            }
        }
    }
}
