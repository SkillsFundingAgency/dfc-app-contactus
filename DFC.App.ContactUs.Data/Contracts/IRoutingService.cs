using DFC.App.ContactUs.Data.Models;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IRoutingService
    {
        Task<RoutingDetailModel?> GetAsync(string searchClue);
    }
}