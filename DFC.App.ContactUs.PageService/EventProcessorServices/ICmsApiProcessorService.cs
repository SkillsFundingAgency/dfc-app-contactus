using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.PageService.EventProcessorServices
{
    public interface ICmsApiProcessorService
    {
        Task<string?> GetDataFromApiAsync(Uri url, string acceptHeader);
    }
}