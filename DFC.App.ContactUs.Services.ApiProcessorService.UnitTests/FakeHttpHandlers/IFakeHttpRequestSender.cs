using System.Net.Http;

namespace DFC.App.ContactUs.Services.ApiProcessorService.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}