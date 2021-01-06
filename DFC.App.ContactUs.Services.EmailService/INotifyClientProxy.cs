using Notify.Interfaces;

namespace DFC.App.ContactUs.Services.EmailService
{
    public interface INotifyClientProxy
    {
        INotificationClient GetNotificationClient();
    }
}
