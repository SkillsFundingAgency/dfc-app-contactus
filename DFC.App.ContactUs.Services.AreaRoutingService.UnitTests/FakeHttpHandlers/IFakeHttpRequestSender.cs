using System.Net.Http;

namespace DFC.App.ContactUs.Services.AreaRoutingService.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}
