using Microsoft.Azure.ServiceBus;

namespace DFC.App.ContactUs.MessageFunctionApp.Services
{
    public interface IMessagePropertiesService
    {
        long GetSequenceNumber(Message message);
    }
}