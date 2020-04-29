using DFC.App.ContactUs.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.MessageFunctionApp.Services
{
    public interface IHttpClientService
    {
        Task<HttpStatusCode> PostAsync(ContentPageModel contentPageModel);

        Task<HttpStatusCode> PutAsync(ContentPageModel contentPageModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}