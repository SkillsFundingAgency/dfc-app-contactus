using DFC.App.ContactUs.Enums;
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
    public class WhyContactUsController : BasePagesController<WhyContactUsController>
    {
        public const string ThisViewCanonicalName = "why-do-you-want-to-contact-us";

        public WhyContactUsController(ILogger<WhyContactUsController> logger, ISessionStateService<SessionDataModel> sessionStateService) : base(logger, sessionStateService)
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
        public async Task<IActionResult> WhyContactUsView(WhyContactUsBodyViewModel? model)
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
        public async Task<IActionResult> WhyContactUsBody(WhyContactUsBodyViewModel? viewModel)
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
                        if (await SetSessionStateAsync(viewModel.SelectedCategory.Value, viewModel.MoreDetail).ConfigureAwait(false))
                        {
                            return Redirect($"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}");
                        }

                        break;
                }
            }

            Logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}