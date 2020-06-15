using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IApiDataProcessorService
    {
        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, Uri url)
            where TApiModel : class;
    }
}