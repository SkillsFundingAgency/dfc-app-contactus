using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class SharedContentCacheReloadService : ISharedContentCacheReloadService
    {
        private readonly ILogger<SharedContentCacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService<ContentPageModel> eventMessageService;
        private readonly IContentCacheService contentCacheService;
        private readonly IContentApiService<ContactUsSummaryItemModel> cmsApiService;

        public SharedContentCacheReloadService(ILogger<SharedContentCacheReloadService> logger, AutoMapper.IMapper mapper, IEventMessageService<ContentPageModel> eventMessageService, IContentApiService<ContactUsSummaryItemModel> cmsApiService, IContentCacheService contentCacheService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.eventMessageService = eventMessageService;
            this.cmsApiService = cmsApiService;
            this.contentCacheService = contentCacheService;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload cache started");

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload cache cancelled");

                    return;
                }

                var summaryList = await GetSummaryListAsync().ConfigureAwait(false);

                if (summaryList != null && summaryList.Any())
                {
                    await ProcessSummaryListAsync(summaryList, stoppingToken).ConfigureAwait(false);

                    if (stoppingToken.IsCancellationRequested)
                    {
                        logger.LogWarning("Reload cache cancelled");

                        return;
                    }

                    await DeleteStaleCacheEntriesAsync(summaryList, stoppingToken).ConfigureAwait(false);
                }

                logger.LogInformation("Reload cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in cache reload");
            }
        }

        public async Task<IList<ContactUsSummaryItemModel>?> GetSummaryListAsync()
        {
            logger.LogInformation("Get summary list");

            // TODO - Filter shared content down by items Contact Us cares about
            // Correct key being passed to shared-content?
            var summaryList = await cmsApiService.GetAll("shared-content").ConfigureAwait(false);

            logger.LogInformation("Get summary list completed");

            return summaryList.ToList();
        }

        public async Task ProcessSummaryListAsync(IList<ContactUsSummaryItemModel>? summaryList, CancellationToken stoppingToken)
        {
            logger.LogInformation("Process summary list started");

            contentCacheService.Clear();

            foreach (var item in summaryList.OrderBy(o => o.Published))
            {
                if (item == null || item.Url == null)
                {
                    continue;
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Process summary list cancelled");

                    return;
                }

                await GetAndSaveItemAsync(item.Url, stoppingToken).ConfigureAwait(false);
            }

            logger.LogInformation("Process summary list completed");
        }

        public async Task GetAndSaveItemAsync(Uri url, CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation($"Get details for {url}");

                var apiDataModel = await cmsApiService.GetById(url).ConfigureAwait(false);

                if (apiDataModel == null)
                {
                    logger.LogWarning($"No details returned from {url}");

                    return;
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Process item get and save cancelled");

                    return;
                }

                var contentPageModel = mapper.Map<ContentPageModel>(apiDataModel);

                if (!TryValidateModel(contentPageModel))
                {
                    logger.LogError($"Validation failure for {url}");

                    return;
                }

                logger.LogInformation($"Updating cache with {url}");

                var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

                if (result == HttpStatusCode.NotFound)
                {
                    logger.LogInformation($"Does not exist, creating cache with {url}");

                    result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

                    if (result == HttpStatusCode.Created)
                    {
                        logger.LogInformation($"Created cache with {url}");
                    }
                    else
                    {
                        logger.LogError($"Cache create error status {result} from {url}");
                    }
                }
                else
                {
                    logger.LogInformation($"Updated cache with {url}");
                }

                var contentItemIds = contentPageModel.ContentItems.Where(w => w.ItemId.HasValue).Select(s => s.ItemId!.Value).ToList();
                contentCacheService.AddOrReplace(contentPageModel.Id, contentItemIds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error in get and save for {url}");
            }
        }

        public async Task DeleteStaleCacheEntriesAsync(IList<ContactUsSummaryItemModel> summaryList, CancellationToken stoppingToken)
        {
            logger.LogInformation("Delete stale cache items started");

            var cachedContentPages = await eventMessageService.GetAllCachedCanonicalNamesAsync().ConfigureAwait(false);

            if (cachedContentPages != null && cachedContentPages.Any())
            {
                var hashedSummaryList = new HashSet<string>(summaryList.Select(p => p.CanonicalName!));
                var staleContentPages = cachedContentPages.Where(p => !hashedSummaryList.Contains(p.CanonicalName!)).ToList();

                if (staleContentPages != null && staleContentPages.Any())
                {
                    await DeleteStaleItemsAsync(staleContentPages, stoppingToken).ConfigureAwait(false);
                }
            }

            logger.LogInformation("Delete stale cache items completed");
        }

        public async Task DeleteStaleItemsAsync(List<ContentPageModel> staleItems, CancellationToken stoppingToken)
        {
            _ = staleItems ?? throw new ArgumentNullException(nameof(staleItems));

            foreach (var staleContentPage in staleItems)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Delete stale cache items cancelled");

                    return;
                }

                logger.LogInformation($"Deleting cache with {staleContentPage.CanonicalName} - {staleContentPage.Id}");

                var deletionResult = await eventMessageService.DeleteAsync(staleContentPage.Id).ConfigureAwait(false);

                if (deletionResult == HttpStatusCode.OK)
                {
                    logger.LogInformation($"Deleted stale cache item {staleContentPage.CanonicalName} - {staleContentPage.Id}");
                }
                else
                {
                    logger.LogError($"Cache delete error status {deletionResult} from {staleContentPage.CanonicalName} - {staleContentPage.Id}");
                }
            }
        }

        public bool TryValidateModel(ContentPageModel contentPageModel)
        {
            _ = contentPageModel ?? throw new ArgumentNullException(nameof(contentPageModel));

            var validationContext = new ValidationContext(contentPageModel, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(contentPageModel, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {contentPageModel.CanonicalName} - {contentPageModel.Url}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }

        public async Task ReloadCacheItem(Uri url)
        {
            await GetAndSaveItemAsync(url, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
