﻿using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.AreaRoutingService.UnitTests.FakeHttpHandlers
{
    [ExcludeFromCodeCoverage]
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly IFakeHttpRequestSender fakeHttpRequestSender;

        public FakeHttpMessageHandler(IFakeHttpRequestSender fakeHttpRequestSender)
        {
            this.fakeHttpRequestSender = fakeHttpRequestSender;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(fakeHttpRequestSender.Send(request));
        }
    }
}
