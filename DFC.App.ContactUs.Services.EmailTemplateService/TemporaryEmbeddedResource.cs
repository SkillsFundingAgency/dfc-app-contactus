using System.IO;
using System.Reflection;
using System.Text;

namespace DFC.App.ContactUs.Services.EmailTemplateService
{
    public static class TemporaryEmbeddedResource
    {
        public static string GetApiRequestFile(string namespaceAndFileName)
        {
            using var stream = typeof(TemporaryEmbeddedResource).GetTypeInfo().Assembly.GetManifestResourceStream(namespaceAndFileName);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
