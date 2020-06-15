using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ICmsApiService
    {
        Task<IList<ContactUsSummaryItemModel>?> GetSummaryAsync();

        Task<ContactUsApiDataModel?> GetItemAsync(Uri url);

        Task<ContactUsApiContentItemModel?> GetContentItemAsync(Uri url);
    }
}
