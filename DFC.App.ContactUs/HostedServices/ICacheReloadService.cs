using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.PageService.EventProcessorServices.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    public interface ICacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);

        Task<IList<ContactUsSummaryItemModel>?> GetSummaryListAsync();

        Task ProcessSummaryListAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken);

        Task GetAndSaveItemAsync(ContactUsSummaryItemModel item, CancellationToken stoppingToken);

        Task DeleteStaleCacheEntriesAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken);
    }
}