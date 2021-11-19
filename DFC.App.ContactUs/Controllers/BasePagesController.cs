using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public abstract class BasePagesController<TController> : Controller
        where TController : Controller
    {
        public const string RegistrationPath = "contact-us";
        public const string WebchatRegistrationPath = "webchat";
        public const string LocalPath = "pages";
        public const string ContactUsPageTitleSuffix = "Contact us | National Careers Service";
        public const string PageTitleSuffix = " | " + ContactUsPageTitleSuffix;

        protected BasePagesController(ILogger<TController> logger, ISessionStateService<SessionDataModel> sessionStateService)
        {
            Logger = logger;
            SessionStateService = sessionStateService;
        }

        protected ILogger<TController> Logger { get; private set; }

        protected ISessionStateService<SessionDataModel> SessionStateService { get; private set; }

        protected static BreadcrumbViewModel BuildBreadcrumb(string segmentPath, BreadcrumbItemModel? breadcrumbItemModel)
        {
            const string BradcrumbTitle = "Contact us";
            var viewModel = new BreadcrumbViewModel
            {
                Breadcrumbs = new List<BreadcrumbItemViewModel>()
                {
                    new BreadcrumbItemViewModel()
                    {
                        Route = "/",
                        Title = "Home",
                    },
                    new BreadcrumbItemViewModel()
                    {
                        Route = $"/{segmentPath}",
                        Title = BradcrumbTitle,
                    },
                },
            };

            if (breadcrumbItemModel?.Title != null &&
                !breadcrumbItemModel.Title.Equals(BradcrumbTitle, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(breadcrumbItemModel.Route))
            {
                var articlePathViewModel = new BreadcrumbItemViewModel
                {
                    Route = $"/{segmentPath}/{breadcrumbItemModel.Route}",
                    Title = breadcrumbItemModel.Title,
                };

                viewModel.Breadcrumbs.Add(articlePathViewModel);
            }

            viewModel.Breadcrumbs.Last().AddHyperlink = false;

            return viewModel;
        }

        protected async Task<SessionStateModel<SessionDataModel>?> GetSessionStateAsync()
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Getting the session state - compositeSessionId = {compositeSessionId}");

                var sessionStateModel = await SessionStateService.GetAsync(compositeSessionId.Value).ConfigureAwait(false);
                if (sessionStateModel != null)
                {
                    return sessionStateModel;
                }

                Logger.LogError($"Error getting the session state - compositeSessionId = {compositeSessionId}");
            }
            else
            {
                Logger.LogWarning("compositeSessionId is null - unable to retrieve session state");
            }

            return default;
        }

        protected async Task<bool> SetSessionStateAsync(Category category, string? moreDetail = null, bool isCallback = false)
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Getting the session state - compositeSessionId = {compositeSessionId}");

                var sessionStateModel = await SessionStateService.GetAsync(compositeSessionId.Value).ConfigureAwait(false);
                sessionStateModel.Ttl = 43200;
                sessionStateModel.State!.Category = category;
                sessionStateModel.State!.MoreDetail = moreDetail;
                sessionStateModel.State!.IsCallback = isCallback;

                Logger.LogInformation($"Saving the session state - compositeSessionId = {compositeSessionId}");

                var result = await SessionStateService.SaveAsync(sessionStateModel).ConfigureAwait(false);

                if (result == HttpStatusCode.OK || result == HttpStatusCode.Created)
                {
                    return true;
                }

                Logger.LogError($"Error saving the session state - compositeSessionId = {compositeSessionId}");
            }
            else
            {
                Logger.LogWarning("compositeSessionId is null - unable to save session state");
            }

            return false;
        }

        protected async Task<bool> DeleteSessionStateAsync()
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Deleting the session state - compositeSessionId = {compositeSessionId}");

                var deleted = await SessionStateService.DeleteAsync(compositeSessionId.Value).ConfigureAwait(false);
                if (deleted)
                {
                    return true;
                }

                Logger.LogError($"Error deleting the session state - compositeSessionId = {compositeSessionId}");
            }
            else
            {
                Logger.LogWarning("compositeSessionId is null - unable to delete session state");
            }

            return false;
        }
    }
}