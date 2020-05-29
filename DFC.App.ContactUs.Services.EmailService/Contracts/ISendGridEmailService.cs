using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailService.Contracts
{
    public interface ISendGridEmailService<in TEmailRequestModel>
        where TEmailRequestModel : class, IEmailRequestModel
    {
        Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel);
    }
}