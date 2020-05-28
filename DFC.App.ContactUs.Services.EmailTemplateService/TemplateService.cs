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

            var tempFolder = @"E:\Source\Repos\SkillsFundingAgency\dfc-app-contactus\DFC.App.ContactUs.Services.EmailTemplateService\Temp\EmailTemplates";
            var templateFile = $@"{tempFolder}\{templateName}.html";

            if (System.IO.File.Exists(templateFile))
            {
                var template = await System.IO.File.ReadAllTextAsync(templateFile).ConfigureAwait(false);

                logger.LogInformation($"{nameof(GetTemplateByNameAsync)} loaded email template: {templateName}");

                return template;
            }

            return null;
        }
    }
}
