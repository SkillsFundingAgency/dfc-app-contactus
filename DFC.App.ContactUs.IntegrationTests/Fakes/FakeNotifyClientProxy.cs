using DFC.App.ContactUs.Services.EmailService;
using Notify.Models.Responses;
using System.Collections.Generic;

namespace DFC.App.ContactUs.IntegrationTests.Fakes
{
    public class FakeNotifyClientProxy : INotifyClientProxy
    {
        public EmailNotificationResponse SendEmail(string toEmail, string templateId, Dictionary<string, dynamic> personalisation)
        {
            return new EmailNotificationResponse();
        }
    }
}
