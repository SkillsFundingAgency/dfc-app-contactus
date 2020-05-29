using DFC.App.ContactUs.Services.EmailTemplateService.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailTemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ILogger<TemplateService> logger;

        public TemplateService(ILogger<TemplateService> logger)
        {
            this.logger = logger;
        }

        public async Task<string?> GetTemplateByNameAsync(string templateName)
        {
            logger.LogInformation($"{nameof(GetTemplateByNameAsync)} loading email template: {templateName}");

            var template = TemporaryEmbeddedResource.GetApiRequestFile($"{this.GetType().Namespace}.Temp.EmailTemplates.{templateName}.html");

            logger.LogInformation($"{nameof(GetTemplateByNameAsync)} loaded email template: {templateName}");

            return template;
        }
    }
}
