using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DFC.App.ContactUs.Framework
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private const string CorrelationId = "DssCorrelationId";

        private readonly IHttpContextAccessor httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetId()
        {
            var result = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.Request.Headers[CorrelationId].FirstOrDefault();
            }

            return result;
        }
    }
}
