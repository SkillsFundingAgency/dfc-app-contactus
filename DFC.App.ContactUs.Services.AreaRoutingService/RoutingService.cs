using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RoutingService> logger;

        public RoutingService(FamApiRoutingOptions famApiRoutingOptions, IApiDataProcessorService apiDataProcessorService, HttpClient httpClient, ILogger<RoutingService> logger)
        {
            this.famApiRoutingOptions = famApiRoutingOptions;
            this.apiDataProcessorService = apiDataProcessorService;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<string> GetEmailToSendTo(string postCode, Category contactCategory)
        {
            switch (contactCategory)
            {
                case Category.Website:
                    logger.LogInformation($"{nameof(GetEmailToSendTo)} has been called for catergory {nameof(Category.Website)}");

                    return famApiRoutingOptions.ProblemsEmailAddress;

                case Category.Feedback:
                    logger.LogInformation($"{nameof(GetEmailToSendTo)} has been called for catergory {nameof(Category.Feedback)}");

                    return famApiRoutingOptions.FeebackEmailAddress;

                case Category.Other:
                    logger.LogInformation($"{nameof(GetEmailToSendTo)} has been called for catergory {nameof(Category.Other)}");

                    return famApiRoutingOptions.OtherEmailAddress;

                case Category.AdviceGuidance:
                case Category.Courses:
                    logger.LogInformation($"{nameof(GetEmailToSendTo)} has been called for catergory {nameof(Category.Courses)}");

                    return await GetFamRoutingEmailAddress(postCode).ConfigureAwait(false);

                default:
                    throw new InvalidEnumArgumentException(nameof(contactCategory), (int)contactCategory, contactCategory.GetType());
            }
        }

        private async Task<string> GetFamRoutingEmailAddress(string postCode)
        {
            logger.LogInformation($"{nameof(GetFamRoutingEmailAddress)} has been called");

            var url = new Uri($"{famApiRoutingOptions.BaseAddress}{famApiRoutingOptions.AreaRoutingEndpoint}{postCode}", UriKind.Absolute);
            var famRouting = await apiDataProcessorService.GetAsync<RoutingDetailModel>(httpClient, url).ConfigureAwait(false);
            return famRouting?.EmailAddress ?? famApiRoutingOptions.FallbackEmailToAddresses;
        }
    }
}