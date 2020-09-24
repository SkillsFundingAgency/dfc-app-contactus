using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ITemplateService
    {
        Task<string?> GetTemplateByKeyAsync(Guid templateKey);
    }
}
