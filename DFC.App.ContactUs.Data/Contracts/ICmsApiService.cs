using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ICmsApiService
    {
        Task<IList<TModel>?> GetAll<TModel>()
            where TModel : class;

        Task<TModel?> GetContentItemAsync<TModel>(Uri url)
            where TModel : class;
    }
}
