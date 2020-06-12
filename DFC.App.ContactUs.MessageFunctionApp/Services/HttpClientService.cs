using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.MessageFunctionApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.MessageFunctionApp.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ContentPageClientOptions contentPageClientOptions;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public HttpClientService(ContentPageClientOptions contentPageClientOptions, HttpClient httpClient, ILogger logger)
        {
            this.contentPageClientOptions = contentPageClientOptions;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<HttpStatusCode> PostAsync(ContentPageModel contentPageModel)
        {
            var url = new Uri($"{contentPageClientOptions?.BaseAddress}pages");

            using (var content = new ObjectContent(typeof(ContentPageModel), contentPageModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for POST, Id: {contentPageModel?.Id}.");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutAsync(ContentPageModel contentPageModel)
        {
            var url = new Uri($"{contentPageClientOptions?.BaseAddress}pages");

            using (var content = new ObjectContent(typeof(ContentPageModel), contentPageModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Put type {typeof(ContentPageModel)}, Id: {contentPageModel?.Id}");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var url = new Uri($"{contentPageClientOptions?.BaseAddress}pages/{id}");
            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Delete type {typeof(ContentPageModel)}, Id: {id}");
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }
    }
}