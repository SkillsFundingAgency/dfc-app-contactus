using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEmailReloadService
    {
        Task Reload(CancellationToken stoppingToken);
    }
}
