using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.AreaRoutingService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies;
using DFC.App.ContactUs.Services.EmailService.Contracts;
using DFC.App.ContactUs.Services.EmailTemplateService.Contracts;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class EnterYourDetailsController : BasePagesController<EnterYourDetailsController>
    {
        public const string ThisViewCanonicalName = "enter-your-details";

        private readonly AutoMapper.IMapper mapper;
        private readonly ISendGridEmailService<ContactUsEmailRequestModel> sendGridEmailService;
        private readonly IRoutingService routingService;
        private readonly FamApiRoutingOptions famApiRoutingOptions;
        private readonly ITemplateService templateService;

        public EnterYourDetailsController(ILogger<EnterYourDetailsController> logger, AutoMapper.IMapper mapper, ISessionStateService<SessionDataModel> sessionStateService, IRoutingService routingService, ISendGridEmailService<ContactUsEmailRequestModel> sendGridEmailService, FamApiRoutingOptions famApiRoutingOptions, ITemplateService templateService) : base(logger, sessionStateService)
        {
            this.mapper = mapper;
            this.routingService = routingService;
            this.sendGridEmailService = sendGridEmailService;
            this.famApiRoutingOptions = famApiRoutingOptions;
            this.templateService = templateService;
        }

        [HttpGet]
        [Route("pages/enter-your-details")]
        public async Task<IActionResult> EnterYourDetailsView()
        {
            var sessionStateModel = await GetSessionStateAsync().ConfigureAwait(false);
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Enter your details",
            };
            var viewModel = new EnterYourDetailsViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = "Enter your details" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                EnterYourDetailsBodyViewModel = new EnterYourDetailsBodyViewModel
                {
                    SelectedCategory = sessionStateModel?.State?.Category ?? Category.None,
                    MoreDetail = sessionStateModel?.State?.MoreDetail,
                },
            };

            Logger.LogWarning($"{nameof(EnterYourDetailsView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/enter-your-details")]
        public async Task<IActionResult> EnterYourDetailsView(EnterYourDetailsBodyViewModel? model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (await SendEmailAsync(model).ConfigureAwait(false))
                {
                    await DeleteSessionStateAsync().ConfigureAwait(false);
                    return Redirect($"/{LocalPath}/{HomeController.ThankyouForContactingUsCanonicalName}");
                }

                ModelState.AddModelError(string.Empty, "Unable to send message, please try again shortly");
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
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
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
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
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
        public async Task<IActionResult> EnterYourDetailsBody()
        {
            var sessionStateModel = await GetSessionStateAsync().ConfigureAwait(false);
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = sessionStateModel?.State?.Category ?? Category.None,
                MoreDetail = sessionStateModel?.State?.MoreDetail,
            };

            Logger.LogInformation($"{nameof(EnterYourDetailsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/enter-your-details/body")]
        public async Task<IActionResult> EnterYourDetailsBody(EnterYourDetailsBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                if (await SendEmailAsync(viewModel).ConfigureAwait(false))
                {
                    await DeleteSessionStateAsync().ConfigureAwait(false);
                    return Redirect($"/{RegistrationPath}/{HomeController.ThankyouForContactingUsCanonicalName}");
                }

                ModelState.AddModelError(string.Empty, "Unable to send message, please try again shortly");
            }

            Logger.LogInformation($"{nameof(EnterYourDetailsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        private async Task<bool> SendEmailAsync(EnterYourDetailsBodyViewModel model)
        {
            Logger.LogInformation($"{nameof(SendEmailAsync)} preparing email");

            var templateName = model.SelectedCategory == Category.Callback ? "CallbackTemplate" : "OnlineMessageTemplate";
            var template = await templateService.GetTemplateByNameAsync(templateName).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(template))
            {
                Logger.LogError($"{nameof(SendEmailAsync)} failed to load email template: {templateName}");
                return false;
            }

            var routingDetailModel = await routingService.GetAsync(model.Postcode!).ConfigureAwait(false);
            var contactUsRequestModel = mapper.Map<ContactUsEmailRequestModel>(model);

            if (model.SelectedCategory == Category.Callback)
            {
                contactUsRequestModel.FromEmailAddress = famApiRoutingOptions.NoReplyEmailAddress;
            }

            contactUsRequestModel.ToEmailAddress = routingDetailModel?.EmailAddress ?? famApiRoutingOptions.FallbackEmailToAddresses;
            contactUsRequestModel.Body = template;
            contactUsRequestModel.BodyNoHtml = null;

            return await sendGridEmailService.SendEmailAsync(contactUsRequestModel).ConfigureAwait(false);
        }
    }
}