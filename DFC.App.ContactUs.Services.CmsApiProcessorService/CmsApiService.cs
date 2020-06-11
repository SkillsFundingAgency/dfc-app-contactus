using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.HttpClientPolicies;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
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

        public async Task<IList<ContactUsSummaryItemModel>?> GetSummaryAsync()
        {
            var url = new Uri($"{cmsApiClientOptions.BaseAddress}{cmsApiClientOptions.SummaryEndpoint}", UriKind.Absolute);

            return await apiDataProcessorService.GetAsync<IList<ContactUsSummaryItemModel>>(httpClient, url).ConfigureAwait(false);
        }

        public async Task<ContactUsApiDataModel?> GetItemAsync(Uri url)
        {
            var contactUsApiDataModel = await apiDataProcessorService.GetAsync<ContactUsApiDataModel>(httpClient, url).ConfigureAwait(false);

            if (contactUsApiDataModel?.ContentItemUrls != null)
            {
                foreach (var contentItemUrl in contactUsApiDataModel.ContentItemUrls)
                {
                    var contactUsApiContentItemModel = await apiDataProcessorService.GetAsync<ContactUsApiContentItemModel>(httpClient, contentItemUrl).ConfigureAwait(false);

                    if (contactUsApiContentItemModel != null)
                    {
                        contactUsApiDataModel.ContentItems.Add(contactUsApiContentItemModel);
                    }
                }
            }

            return contactUsApiDataModel;
        }
    }
}
