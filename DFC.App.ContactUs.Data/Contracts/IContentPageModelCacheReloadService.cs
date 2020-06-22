using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ISharedContentCacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);

        Task ReloadCacheItem(Uri url);

        Task<IList<ContactUsSummaryItemModel>?> GetSummaryListAsync();

        Task ProcessSummaryListAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken);

        Task GetAndSaveItemAsync(Uri url, CancellationToken stoppingToken);

        Task DeleteStaleItemsAsync(List<ContentPageModel> staleItems, CancellationToken stoppingToken);

        Task DeleteStaleCacheEntriesAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken);
    }
}