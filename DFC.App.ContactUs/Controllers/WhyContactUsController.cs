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
        public WhyContactUsController(ILogger<WhyContactUsController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us")]
        public IActionResult WhyContactUsView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "why-do-you-want-to-contact-us",
                BreadcrumbTitle = "Why do you want to contact us",
            };
            var viewModel = new WhyContactUsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/why-do-you-want-to-contact-us", UriKind.RelativeOrAbsolute),
                    Title = "Why do you want to contact us | Contact us | National Careers Service",
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                WhyContactUsBodyViewModel = new WhyContactUsBodyViewModel(),
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
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/why-do-you-want-to-contact-us", UriKind.RelativeOrAbsolute),
                Title = "Why do you want to contact us | Contact us | National Careers Service",
            };

            Logger.LogInformation($"{nameof(WhyContactUsHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/why-do-you-want-to-contact-us/breadcrumb")]
        public IActionResult WhyContactUsBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "why-do-you-want-to-contact-us",
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
                    case WhyContactUsBodyViewModel.SelectCategory.AdviceGuidance:
                        return Redirect($"/{RegistrationPath}");
                    case WhyContactUsBodyViewModel.SelectCategory.Courses:
                        return Redirect($"/{RegistrationPath}");
                    case WhyContactUsBodyViewModel.SelectCategory.Website:
                        return Redirect($"/{RegistrationPath}");
                    case WhyContactUsBodyViewModel.SelectCategory.Feedback:
                        return Redirect($"/{RegistrationPath}");
                    case WhyContactUsBodyViewModel.SelectCategory.SomethingElse:
                        return Redirect($"/{RegistrationPath}");
                }
            }

            Logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}