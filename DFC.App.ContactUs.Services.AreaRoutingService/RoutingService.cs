﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using System;
using System.ComponentModel;
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

        public async Task<string> GetEmailToSendTo(string postCode, Category contactCategory)
        {
            switch (contactCategory)
            {
                case Category.Website:
                    return famApiRoutingOptions.ProblemsEmailAddress;

                case Category.Feedback:
                    return famApiRoutingOptions.FeebackEmailAddress;

                case Category.Other:
                    return famApiRoutingOptions.OtherEmailAddress;

                case Category.AdviceGuidance:
                case Category.Courses:
                    return await GetFamRoutingEmailAddress(postCode).ConfigureAwait(false);

                default:
                    throw new InvalidEnumArgumentException(nameof(contactCategory), (int)contactCategory, contactCategory.GetType());
            }
        }

        private async Task<string> GetFamRoutingEmailAddress(string postCode)
        {
            var url = new Uri($"{famApiRoutingOptions.BaseAddress}{famApiRoutingOptions.AreaRoutingEndpoint}{postCode}", UriKind.Absolute);
            var famRouting = await apiDataProcessorService.GetAsync<RoutingDetailModel>(httpClient, url).ConfigureAwait(false);
            return famRouting?.EmailAddress ?? famApiRoutingOptions.FallbackEmailToAddresses;
        }
    }
}