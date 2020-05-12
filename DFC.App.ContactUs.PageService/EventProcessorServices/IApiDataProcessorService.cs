using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.PageService.EventProcessorServices
{
    public interface IApiDataProcessorService
    {
        Task<TApiModel?> GetAsync<TApiModel>(Uri url)
            where TApiModel : class;
    }
}