using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.PageService.EventProcessorServices
{
    public class ApiDataProcessorService : IApiDataProcessorService
    {
        private readonly ICmsApiProcessorService cmsApiProcessorService;

        public ApiDataProcessorService(ICmsApiProcessorService cmsApiProcessorService)
        {
            this.cmsApiProcessorService = cmsApiProcessorService;
        }

        public async Task<TApiModel?> GetAsync<TApiModel>(Uri url)
            where TApiModel : class
        {
            var response = await cmsApiProcessorService.GetDataFromApiAsync(url, MediaTypeNames.Application.Json).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(response))
            {
                var apiDataModel = JsonConvert.DeserializeObject<TApiModel>(response);

                return apiDataModel;
            }

            return default;
        }
    }
}
