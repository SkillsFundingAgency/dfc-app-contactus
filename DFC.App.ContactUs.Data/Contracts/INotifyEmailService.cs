using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface INotifyEmailService<in TEmailRequestModel>
       where TEmailRequestModel : class, IEmailRequestModel
    {
        Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel);
    }
}
