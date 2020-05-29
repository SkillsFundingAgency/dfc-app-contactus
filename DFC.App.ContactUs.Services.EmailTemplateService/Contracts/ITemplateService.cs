using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailTemplateService.Contracts
{
    public interface ITemplateService
    {
        Task<string?> GetTemplateByNameAsync(string templateName);
    }
}
