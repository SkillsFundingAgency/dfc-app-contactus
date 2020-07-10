using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IContentApiService<TApiResponseModel>
        where TApiResponseModel : class
    {
        Task<IEnumerable<TApiResponseModel>> GetAll(string contentType);

        Task<TApiResponseModel> GetById(string contentType, string id);

        Task<TApiResponseModel> GetById(Uri uri);
    }
}