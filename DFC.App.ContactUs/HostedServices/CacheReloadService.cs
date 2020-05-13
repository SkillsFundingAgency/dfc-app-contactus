﻿using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.HttpClientPolicies;
using DFC.App.ContactUs.PageService.EventProcessorServices;
using DFC.App.ContactUs.PageService.EventProcessorServices.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    public class CacheReloadService : ICacheReloadService
    {
        private readonly ILogger<CacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IEventMessageService eventMessageService;
        private readonly IApiDataProcessorService apiDataProcessorService;

        public CacheReloadService(ILogger<CacheReloadService> logger, AutoMapper.IMapper mapper, CmsApiClientOptions cmsApiClientOptions, IEventMessageService eventMessageService, IApiDataProcessorService apiDataProcessorService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.eventMessageService = eventMessageService;
            this.apiDataProcessorService = apiDataProcessorService;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation($"Reload cache started");

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

                logger.LogInformation($"Reload cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error in cache reload");
            }
        }

        public async Task<IList<ContactUsSummaryItemModel>?> GetSummaryListAsync()
        {
            var url = new Uri($"{cmsApiClientOptions.BaseAddress}{cmsApiClientOptions.SummaryEndpoint}");

            logger.LogInformation($"Get summary list from {url}");

            var summaryList = await apiDataProcessorService.GetAsync<IList<ContactUsSummaryItemModel>>(url).ConfigureAwait(false);

            logger.LogInformation($"Get summary list completed");

            return summaryList;
        }

        public async Task ProcessSummaryListAsync(IList<ContactUsSummaryItemModel>? summaryList, CancellationToken stoppingToken)
        {
            logger.LogInformation("Process summary list started");

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
            try
            {
                logger.LogInformation($"Get details for {item.CanonicalName} - {item.Url}");

                var apiDataModel = await apiDataProcessorService.GetAsync<ContactUsApiDataModel>(item.Url!).ConfigureAwait(false);

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

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Process item get and save cancelled");

                    return;
                }

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
            foreach (var staleContentPage in staleItems)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Delete stale cache items cancelled");

                    return;
                }

                logger.LogInformation($"Deleting cache with {staleContentPage.CanonicalName} - {staleContentPage.DocumentId}");

                var deletionResult = await eventMessageService.DeleteAsync(staleContentPage.DocumentId).ConfigureAwait(false);

                if (deletionResult == HttpStatusCode.OK)
                {
                    logger.LogInformation($"Deleted stale cache item {staleContentPage.CanonicalName} - {staleContentPage.DocumentId}");
                }
                else
                {
                    logger.LogError($"Cache delete error status {deletionResult} from {staleContentPage.CanonicalName} - {staleContentPage.DocumentId}");
                }
            }
        }

        public bool TryValidateModel(ContentPageModel contentPageModel)
        {
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