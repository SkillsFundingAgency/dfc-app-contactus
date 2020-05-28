using DFC.App.ContactUs.Services.EmailService.Contracts;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.Services.EmailService
{
    public class SendGridEmailService<TEmailRequestModel> : ISendGridEmailService<TEmailRequestModel>
        where TEmailRequestModel : class, IEmailRequestModel
    {
        private readonly ILogger<SendGridEmailService<TEmailRequestModel>> logger;
        private readonly IMergeEmailContentService mergeEmailContentService;
        private readonly ISendGridClient sendGridClient;

        public SendGridEmailService(
            ILogger<SendGridEmailService<TEmailRequestModel>> logger,
            IMergeEmailContentService mergeEmailContentService,
            ISendGridClient sendGridClient)
        {
            this.logger = logger;
            this.mergeEmailContentService = mergeEmailContentService;
            this.sendGridClient = sendGridClient;
        }

        public virtual async Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel)
        {
            _ = emailRequestModel ?? throw new ArgumentNullException(nameof(emailRequestModel));

            logger.LogInformation($"{nameof(SendEmailAsync)} sending email to {emailRequestModel.ToEmailAddress}");

            var from = new EmailAddress(emailRequestModel.FromEmailAddress, $"{emailRequestModel.GivenName} {emailRequestModel.FamilyName}");
            var subject = emailRequestModel.Subject;
            var to = emailRequestModel.ToEmailAddress?.Split(';').Select(toEmail => new EmailAddress(toEmail.Trim(), toEmail.Trim())).ToList();
            var plainTextContent = mergeEmailContentService.MergeTemplateBodyWithContent(emailRequestModel, emailRequestModel.BodyNoHtml);
            var htmlContent = mergeEmailContentService.MergeTemplateBodyWithContent(emailRequestModel, emailRequestModel.Body);
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);

            return true;                 //TODO: ian: enable email in Sprint 6 - DFCC 267

            //var clientResponse = await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            //var result = clientResponse.StatusCode.Equals(HttpStatusCode.Accepted);

            //if (result)
            //{
            //    logger.LogInformation($"{nameof(SendEmailAsync)} sent email to {emailRequestModel.ToEmailAddress}");
            //}
            //else
            //{
            //    logger.LogWarning($"{nameof(SendEmailAsync)} failed to send email to {emailRequestModel.ToEmailAddress}");
            //}

            //return result;
        }
    }
}
