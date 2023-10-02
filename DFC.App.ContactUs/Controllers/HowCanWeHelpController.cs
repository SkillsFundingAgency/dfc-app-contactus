using DFC.App.ContactUs.Data.Enums;
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
        private readonly ILogger<HowCanWeHelpController> logger;

        public HowCanWeHelpController(ILogger<HowCanWeHelpController> logger, ISessionStateService<SessionDataModel> sessionStateService) : base(logger, sessionStateService)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("pages/how-can-we-help")]
        public async Task<IActionResult> HowCanWeHelpView()
        {
            logger.LogInformation($"{nameof(HowCanWeHelpView)} has been called");

            var sessionStateModel = await GetSessionStateAsync().ConfigureAwait(false);
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = Title,
            };
            var viewModel = new HowCanWeHelpViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = Title + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HowCanWeHelpBodyViewModel = new HowCanWeHelpBodyViewModel
                {
                    SelectedCategory = sessionStateModel?.State?.Category ?? Category.None,
                    MoreDetail = sessionStateModel?.State?.MoreDetail,
                    IsCallback = sessionStateModel?.State?.IsCallback ?? false,
                },
            };

            Logger.LogWarning($"{nameof(HowCanWeHelpView)} has returned content");
            Logger.LogWarning($"{nameof(HowCanWeHelpViewModel)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/how-can-we-help")]
        public async Task<IActionResult> HowCanWeHelpView(HowCanWeHelpBodyViewModel? model)
        {
            logger.LogInformation($"{nameof(HowCanWeHelpView)} has been called");

            if (model != null && ModelState.IsValid)
            {
                switch (model.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.Other:
                        if (await SetSessionStateAsync(model.SelectedCategory.Value, model.MoreDetail, model.IsCallback).ConfigureAwait(false))
                        {
                            return Redirect($"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}");
                        }

                        break;
                }
            }

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = Title,
            };
            var viewModel = new HowCanWeHelpViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = Title + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HowCanWeHelpBodyViewModel = model,
            };

            Logger.LogWarning($"{nameof(HowCanWeHelpView)} has returned content");
            Logger.LogWarning($"{nameof(HowCanWeHelpViewModel)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/how-can-we-help/head")]
        public IActionResult HowCanWeHelpHead()
        {
            logger.LogInformation($"{nameof(HowCanWeHelpHead)} has been called");

            var viewModel = new HeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                Title = Title + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(HowCanWeHelpHead)} has returned content");
            Logger.LogWarning($"{nameof(HeadViewModel)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/how-can-we-help/breadcrumb")]
        public IActionResult HowCanWeHelpBreadcrumb()
        {
            logger.LogInformation($"{nameof(HowCanWeHelpBreadcrumb)} has been called");

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = Title,
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(HowCanWeHelpBreadcrumb)} has returned content");
            Logger.LogWarning($"{nameof(BreadcrumbItemModel)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/how-can-we-help/body")]
        public async Task<IActionResult> HowCanWeHelpBody()
        {
            logger.LogInformation($"{nameof(HowCanWeHelpBody)} has been called");

            var sessionStateModel = await GetSessionStateAsync().ConfigureAwait(false);
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = sessionStateModel?.State?.Category ?? Category.None,
                MoreDetail = sessionStateModel?.State?.MoreDetail,
                IsCallback = sessionStateModel?.State?.IsCallback ?? false,
            };

            Logger.LogInformation($"{nameof(HowCanWeHelpBody)} has returned content");
            Logger.LogWarning($"{nameof(HowCanWeHelpBodyViewModel)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/how-can-we-help/body")]
        public async Task<IActionResult> HowCanWeHelpBody(HowCanWeHelpBodyViewModel? viewModel)
        {
            logger.LogInformation($"{nameof(HowCanWeHelpBody)} has been called");

            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.Other:
                        if (await SetSessionStateAsync(viewModel.SelectedCategory.Value, viewModel.MoreDetail, viewModel.IsCallback).ConfigureAwait(false))
                        {
                            return Redirect($"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}");
                        }

                        break;
                }
            }

            Logger.LogInformation($"{nameof(HowCanWeHelpBody)} has returned content");
            Logger.LogWarning($"{nameof(HowCanWeHelpBody)} model has been generated");

            return this.NegotiateContentResult(viewModel);
        }
    }
}