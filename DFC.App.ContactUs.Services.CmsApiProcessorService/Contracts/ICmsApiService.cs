using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts
{
    public interface ICmsApiService
    {
        Task<IList<ContactUsSummaryItemModel>?> GetSummaryAsync();

        Task<ContactUsApiDataModel?> GetItemAsync(Uri url);
    }
}
