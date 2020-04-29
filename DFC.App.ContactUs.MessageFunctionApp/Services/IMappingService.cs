using DFC.App.ContactUs.Data.Models;

namespace DFC.App.ContactUs.MessageFunctionApp.Services
{
    public interface IMappingService
    {
        ContentPageModel MapToContentPageModel(string message, long sequenceNumber);
    }
}