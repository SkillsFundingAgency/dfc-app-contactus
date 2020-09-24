using System;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Helpers
{
    public static class EmailKeyHelper
    {
        private static Guid CallbackTemplateKey => Guid.Parse("e11a1137-01ca-446a-b60f-0de5ad5321cc");

        private static Guid OnlineMessageTemplateKey => Guid.Parse("e11a1195-801d-479b-84b6-f5e443abfb86");

        public static Guid GetEmailKey(bool isCallback)
        {
            if (isCallback)
            {
                return CallbackTemplateKey;
            }

            return OnlineMessageTemplateKey;
        }

        public static IEnumerable<Guid> GetEmailKeys()
        {
            return new List<Guid>
            {
                CallbackTemplateKey,
                OnlineMessageTemplateKey,
            };
        }
    }
}
