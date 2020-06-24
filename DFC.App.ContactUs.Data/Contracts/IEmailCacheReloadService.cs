using System;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEmailCacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);

        Task ReloadCacheItem(Uri uri);
    }
}
