using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
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

        Task DeleteStaleItemsAsync(List<ContentPageModel> staleItems, CancellationToken stoppingToken);

        Task DeleteStaleCacheEntriesAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken);
    }
}