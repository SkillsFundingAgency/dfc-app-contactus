using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies;
using DFC.App.ContactUs.Services.AreaRoutingService.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.AreaRoutingService
{
    public class RoutingService : IRoutingService
    {
        private readonly FamApiRoutingOptions famApiRoutingOptions;
        private readonly IApiDataProcessorService apiDataProcessorService;
        private readonly HttpClient httpClient;

        public RoutingService(FamApiRoutingOptions famApiRoutingOptions, IApiDataProcessorService apiDataProcessorService, HttpClient httpClient)
        {
            this.famApiRoutingOptions = famApiRoutingOptions;
            this.apiDataProcessorService = apiDataProcessorService;
            this.httpClient = httpClient;
        }

        public async Task<RoutingDetailModel?> GetAsync(string searchClue)
        {
            var url = new Uri($"{famApiRoutingOptions.BaseAddress}{famApiRoutingOptions.AreaRoutingEndpoint}{searchClue}", UriKind.Absolute);

            return await apiDataProcessorService.GetAsync<RoutingDetailModel>(httpClient, url).ConfigureAwait(false);
        }
    }
}