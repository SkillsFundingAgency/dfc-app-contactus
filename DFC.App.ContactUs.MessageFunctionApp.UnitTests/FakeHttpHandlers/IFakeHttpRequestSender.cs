using System.Net.Http;

namespace DFC.App.ContactUs.MessageFunctionApp.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}