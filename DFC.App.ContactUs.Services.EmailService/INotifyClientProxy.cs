using Notify.Interfaces;
using Notify.Models.Responses;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Services.EmailService
{
    public interface INotifyClientProxy
    {
        EmailNotificationResponse SendEmail(string toEmail, string templateId, Dictionary<string, dynamic> personalisation);
    }
}
