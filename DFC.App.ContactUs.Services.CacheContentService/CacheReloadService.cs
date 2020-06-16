﻿using DFC.App.ContactUs.Data.Contracts;
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
    public class CacheReloadService : ICacheReloadService
    {
        private readonly ILogger<CacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService<ContentPageModel> eventMessageService;
        private readonly ICmsApiService cmsApiService;
        private readonly IContentCacheService contentCacheService;

        public CacheReloadService(ILogger<CacheReloadService> logger, AutoMapper.IMapper mapper, IEventMessageService<ContentPageModel> eventMessageService, ICmsApiService cmsApiService, IContentCacheService contentCacheService)
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

                var summaryList = await GetSummaryListAsync().ConfigureAwait(false);

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload cache cancelled");

                    return;
                }

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

            var summaryList = await cmsApiService.GetSummaryAsync().ConfigureAwait(false);

            logger.LogInformation("Get summary list completed");

            return summaryList;
        }

        public async Task ProcessSummaryListAsync(IList<ContactUsSummaryItemModel>? summaryList, CancellationToken stoppingToken)
        {
            logger.LogInformation("Process summary list started");

            contentCacheService.Clear();

            foreach (var item in summaryList.OrderBy(o => o.Published))
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Process summary list cancelled");

                    return;
                }

                await GetAndSaveItemAsync(item, stoppingToken).ConfigureAwait(false);
            }

            logger.LogInformation("Process summary list completed");
        }

        public async Task GetAndSaveItemAsync(ContactUsSummaryItemModel item, CancellationToken stoppingToken)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            try
            {
                logger.LogInformation($"Get details for {item.CanonicalName} - {item.Url}");

                var apiDataModel = await cmsApiService.GetItemAsync(item.Url!).ConfigureAwait(false);

                if (apiDataModel == null)
                {
                    logger.LogWarning($"No details returned from {item.CanonicalName} - {item.Url}");

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
                    logger.LogError($"Validation failure for {item.CanonicalName} - {item.Url}");

                    return;
                }

                logger.LogInformation($"Updating cache with {item.CanonicalName} - {item.Url}");

                var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

                if (result == HttpStatusCode.NotFound)
                {
                    logger.LogInformation($"Does not exist, creating cache with {item.CanonicalName} - {item.Url}");

                    result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);

                    if (result == HttpStatusCode.Created)
                    {
                        logger.LogInformation($"Created cache with {item.CanonicalName} - {item.Url}");
                    }
                    else
                    {
                        logger.LogError($"Cache create error status {result} from {item.CanonicalName} - {item.Url}");
                    }
                }
                else
                {
                    logger.LogInformation($"Updated cache with {item.CanonicalName} - {item.Url}");
                }

                var contentItemIds = contentPageModel.ContentItems.Where(w => w.ItemId.HasValue).Select(s => s.ItemId!.Value).ToList();
                contentCacheService.AddOrReplace(contentPageModel.Id, contentItemIds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error in get and save for {item.CanonicalName} - {item.Url}");
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
    }
}