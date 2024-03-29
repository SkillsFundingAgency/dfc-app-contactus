﻿using DFC.App.ContactUs.Data.Contracts;
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
    public class EnterYourDetailsController : BasePagesController<EnterYourDetailsController>
    {
        public const string ThisViewCanonicalName = "enter-your-details";

        private readonly AutoMapper.IMapper mapper;
        private readonly INotifyEmailService<ContactUsEmailRequestModel> notifyEmailService;
        private readonly IRoutingService routingService;

        public EnterYourDetailsController(ILogger<EnterYourDetailsController> logger, AutoMapper.IMapper mapper, ISessionStateService<SessionDataModel> sessionStateService, IRoutingService routingService, INotifyEmailService<ContactUsEmailRequestModel> notifyEmailService) : base(logger, sessionStateService)
        {
            this.mapper = mapper;
            this.routingService = routingService;
            this.notifyEmailService = notifyEmailService;
        }

        [HttpGet]
        [Route("pages/enter-your-details")]
        public async Task<IActionResult> EnterYourDetailsView()
        {
            var sessionStateModel = await GetSessionStateAsync().ConfigureAwait(false);
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Enter your details",
            };
            var viewModel = new EnterYourDetailsViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                    Title = "Enter your details" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                EnterYourDetailsBodyViewModel = new EnterYourDetailsBodyViewModel
                {
                    SelectedCategory = sessionStateModel?.State?.Category ?? Category.None,
                    MoreDetail = sessionStateModel?.State?.MoreDetail,
                    IsCallback = sessionStateModel?.State?.IsCallback ?? false,
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
                Route = ThisViewCanonicalName,
                Title = "Enter your details",
            };
            var viewModel = new EnterYourDetailsViewModel()
            {
                Head = new HeadViewModel
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
        [Route("pages/enter-your-details/head")]
        public IActionResult EnterYourDetailsHead()
        {
            var viewModel = new HeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{ThisViewCanonicalName}", UriKind.RelativeOrAbsolute),
                Title = "Enter your details" + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(EnterYourDetailsHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/enter-your-details/breadcrumb")]
        public IActionResult EnterYourDetailsBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Enter your details",
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
                IsCallback = sessionStateModel?.State?.IsCallback ?? false,
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
            var contactUsRequestModel = mapper.Map<ContactUsEmailRequestModel>(model);

            contactUsRequestModel.ToEmailAddress = await routingService.GetEmailToSendTo(model.Postcode!, model.SelectedCategory).ConfigureAwait(false);
            return await notifyEmailService.SendEmailAsync(contactUsRequestModel).ConfigureAwait(false);
        }
    }
}