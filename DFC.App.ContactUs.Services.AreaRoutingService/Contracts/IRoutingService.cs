using DFC.App.ContactUs.Services.AreaRoutingService.Models;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.AreaRoutingService.Contracts
{
    public interface IRoutingService
    {
        Task<RoutingDetailModel?> GetAsync(string searchClue);
    }
}