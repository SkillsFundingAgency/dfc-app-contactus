using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface INotifyEmailServices<in TEmailRequestModel>
       where TEmailRequestModel : class, IEmailRequestModel
    {
        Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel);
    }
}
