using DFC.App.ContactUs.Data.Enums;
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
                Paths = new List<BreadcrumbPathViewModel>()
                {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home",
                    },
                    new BreadcrumbPathViewModel()
                    {
                        Route = $"/{segmentPath}",
                        Title = BradcrumbTitle,
                    },
                },
            };

            if (breadcrumbItemModel?.BreadcrumbTitle != null &&
                !breadcrumbItemModel.BreadcrumbTitle.Equals(BradcrumbTitle, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(breadcrumbItemModel.CanonicalName))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{segmentPath}/{breadcrumbItemModel.CanonicalName}",
                    Title = breadcrumbItemModel.BreadcrumbTitle,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        protected async Task<SessionStateModel<SessionDataModel>?> GetSessionStateAsync()
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Getting the session state - compositeSessionId = {compositeSessionId}");

                return await SessionStateService.GetAsync(compositeSessionId.Value).ConfigureAwait(false);
            }

            Logger.LogError($"Error getting the session state - compositeSessionId = {compositeSessionId}");

            return default;
        }

        protected async Task<bool> SetSessionStateAsync(Category category, string? moreDetail = null)
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Getting the session state - compositeSessionId = {compositeSessionId}");

                var sessionStateModel = await SessionStateService.GetAsync(compositeSessionId.Value).ConfigureAwait(false);
                sessionStateModel.Ttl = 43200;
                sessionStateModel.State!.Category = category;
                sessionStateModel.State!.MoreDetail = moreDetail;

                Logger.LogInformation($"Saving the session state - compositeSessionId = {compositeSessionId}");

                var result = await SessionStateService.SaveAsync(sessionStateModel).ConfigureAwait(false);

                return result == HttpStatusCode.OK || result == HttpStatusCode.Created;
            }

            Logger.LogError($"Error saving the session state - compositeSessionId = {compositeSessionId}");

            return false;
        }

        protected async Task<bool> DeleteSessionStateAsync()
        {
            var compositeSessionId = Request.CompositeSessionId();
            if (compositeSessionId.HasValue)
            {
                Logger.LogInformation($"Deleting the session state - compositeSessionId = {compositeSessionId}");

                return await SessionStateService.DeleteAsync(compositeSessionId.Value).ConfigureAwait(false);
            }

            Logger.LogError($"Error deleting the session state - compositeSessionId = {compositeSessionId}");

            return false;
        }
    }
}