using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailTemplateService
{
    public class TemplateService : ITemplateService
    {
        private readonly ILogger<TemplateService> logger;
        private readonly IDocumentService<EmailModel> emailDocumentService;

        public TemplateService(ILogger<TemplateService> logger, IDocumentService<EmailModel> emailDocumentService)
        {
            this.logger = logger;
            this.emailDocumentService = emailDocumentService;
        }

        public async Task<string?> GetTemplateByKeyAsync(Guid templateKey)
        {
            logger.LogInformation($"{nameof(GetTemplateByKeyAsync)} loading email template: {templateKey}");

            var template = await emailDocumentService.GetByIdAsync(templateKey).ConfigureAwait(false);

            if (template == null)
            {
                throw new InvalidOperationException($"Email Template - {templateKey} not found");
            }

            logger.LogInformation($"{nameof(GetTemplateByKeyAsync)} loaded email template: {templateKey}");

            return template.Body;
        }
    }
}
