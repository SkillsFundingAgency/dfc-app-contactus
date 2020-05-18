using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.ContactUs.Controllers
{
    public abstract class BasePagesController<TController> : Controller
        where TController : Controller
    {
        public const string RegistrationPath = "contact-us";
        public const string WebchatRegistrationPath = "webchat";
        public const string LocalPath = "pages";

        protected BasePagesController(ILogger<TController> logger)
        {
            Logger = logger;
        }

        protected ILogger<TController> Logger { get; private set; }

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
    }
}