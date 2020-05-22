using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Controllers
{
    public class EnterYourDetailsController : BasePagesController<EnterYourDetailsController>
    {
        public const string ThisViewCanonicalName = "enter-your-details";

        public EnterYourDetailsController(ILogger<EnterYourDetailsController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("pages/enter-your-details")]
        public IActionResult EnterYourDetailsView(Category category = Category.None)
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Enter your details",
            };
            var viewModel = new EnterYourDetailsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/enter-your-details", UriKind.RelativeOrAbsolute),
                    Title = "Enter your details" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                EnterYourDetailsBodyViewModel = new EnterYourDetailsBodyViewModel { SelectedCategory = category, },
            };

            Logger.LogWarning($"{nameof(EnterYourDetailsView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/enter-your-details")]
        public IActionResult EnterYourDetailsView(EnterYourDetailsBodyViewModel? model)
        {
            if (model != null && ModelState.IsValid)
            {
                return Redirect($"/{LocalPath}");
            }

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Enter your details",
            };
            var viewModel = new EnterYourDetailsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/enter-your-details", UriKind.RelativeOrAbsolute),
                    Title = "Enter your details" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                EnterYourDetailsBodyViewModel = model,
            };

            Logger.LogInformation($"{nameof(EnterYourDetailsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/enter-your-details/htmlhead")]
        public IActionResult EnterYourDetailsHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/enter-your-details", UriKind.RelativeOrAbsolute),
                Title = "Enter your details" + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(EnterYourDetailsHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/enter-your-details/breadcrumb")]
        public IActionResult EnterYourDetailsBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Enter your details",
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(EnterYourDetailsBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/enter-your-details/body")]
        public IActionResult EnterYourDetailsBody(Category category = Category.None)
        {
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = category,
            };

            Logger.LogInformation($"{nameof(EnterYourDetailsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/enter-your-details/body")]
        public IActionResult EnterYourDetailsBody(EnterYourDetailsBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                return Redirect($"/{RegistrationPath}");
            }

            Logger.LogInformation($"{nameof(EnterYourDetailsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}