﻿using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class HowCanWeHelpController : BasePagesController<HowCanWeHelpController>
    {
        public const string ThisViewCanonicalName = "how-can-we-help";

        private const string Title = "How can we help?";

        public HowCanWeHelpController(ILogger<HowCanWeHelpController> logger, ISessionStateService<SessionDataModel> sessionStateService) : base(logger, sessionStateService)
        {
        }

        [HttpGet]
        [Route("pages/how-can-we-help")]
        public IActionResult HowCanWeHelpView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = Title,
            };
            var viewModel = new HowCanWeHelpViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = Title + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HowCanWeHelpBodyViewModel = new HowCanWeHelpBodyViewModel(),
            };

            Logger.LogWarning($"{nameof(HowCanWeHelpView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/how-can-we-help")]
        public async Task<IActionResult> HowCanWeHelpView(HowCanWeHelpBodyViewModel? model)
        {
            if (model != null && ModelState.IsValid)
            {
                switch (model.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.Other:
                        if (await SetSessionStateAsync(model.SelectedCategory.Value, model.MoreDetail).ConfigureAwait(false))
                        {
                            return Redirect($"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}");
                        }

                        break;
                }
            }

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = Title,
            };
            var viewModel = new HowCanWeHelpViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = Title + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HowCanWeHelpBodyViewModel = model,
            };

            Logger.LogWarning($"{nameof(HowCanWeHelpView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/how-can-we-help/htmlhead")]
        public IActionResult HowCanWeHelpHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                Title = Title + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(HowCanWeHelpHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/how-can-we-help/breadcrumb")]
        public IActionResult HowCanWeHelpBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = Title,
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(HowCanWeHelpBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/how-can-we-help/body")]
        public IActionResult HowCanWeHelpBody()
        {
            var viewModel = new HowCanWeHelpBodyViewModel();

            Logger.LogInformation($"{nameof(HowCanWeHelpBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/how-can-we-help/body")]
        public async Task<IActionResult> HowCanWeHelpBody(HowCanWeHelpBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.Other:
                        if (await SetSessionStateAsync(viewModel.SelectedCategory.Value, viewModel.MoreDetail).ConfigureAwait(false))
                        {
                            return Redirect($"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}");
                        }

                        break;
                }
            }

            Logger.LogInformation($"{nameof(HowCanWeHelpBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}