using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Controllers
{
    public class WhyContactUsController : BasePagesController<WhyContactUsController>
    {
        public const string ThisViewCanonicalName = "why-do-you-want-to-contact-us";

        public WhyContactUsController(ILogger<WhyContactUsController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us")]
        public IActionResult WhyContactUsView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Why do you want to contact us",
            };
            var viewModel = new WhyContactUsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = "Why do you want to contact us" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                WhyContactUsBodyViewModel = new WhyContactUsBodyViewModel(),
            };

            Logger.LogWarning($"{nameof(WhyContactUsView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/why-do-you-want-to-contact-us")]
        public IActionResult WhyContactUsView(WhyContactUsBodyViewModel? model)
        {
            if (model != null && ModelState.IsValid)
            {
                switch (model.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.SomethingElse:
                        return Redirect($"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={model.SelectedCategory}");
                }
            }

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Why do you want to contact us",
            };
            var viewModel = new WhyContactUsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = "Why do you want to contact us" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                WhyContactUsBodyViewModel = model,
            };

            Logger.LogWarning($"{nameof(WhyContactUsView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us/htmlhead")]
        public IActionResult WhyContactUsHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                Title = "Why do you want to contact us" + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(WhyContactUsHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/why-do-you-want-to-contact-us/breadcrumb")]
        public IActionResult WhyContactUsBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Why do you want to contact us",
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(WhyContactUsBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us/body")]
        public IActionResult WhyContactUsBody()
        {
            var viewModel = new WhyContactUsBodyViewModel();

            Logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/why-do-you-want-to-contact-us/body")]
        public IActionResult WhyContactUsBody(WhyContactUsBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedCategory)
                {
                    case Category.AdviceGuidance:
                    case Category.Courses:
                    case Category.Website:
                    case Category.Feedback:
                    case Category.SomethingElse:
                        return Redirect($"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={viewModel.SelectedCategory}");
                }
            }

            Logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}