using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Constants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.ContactUs.Controllers
{
    public class HomeController : BasePagesController<HomeController>
    {
        public const string ThisViewCanonicalName = "home";
        public const string SendUsLetterCanonicalName = "send-us-a-letter";
        public const string ThankyouForContactingUsCanonicalName = "thank-you-for-contacting-us";
        private readonly ISharedContentRedisInterface sharedContentRedis;
        private readonly IConfiguration configuration;
        private string status;

        public HomeController(ILogger<HomeController> logger, ISessionStateService<SessionDataModel> sessionStateService, ISharedContentRedisInterface sharedContentRedis, IConfiguration configuration) : base(logger, sessionStateService)
        {
            this.sharedContentRedis = sharedContentRedis;
            this.configuration = configuration;
            status = configuration.GetSection("ContentMode:ContentMode").Get<string>();

            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }
        }

        [HttpGet]
        [Route("pages/home")]
        public async Task<IActionResult> HomeView()
        {
            await DeleteSessionStateAsync().ConfigureAwait(false);

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Contact us",
            };
            var viewModel = new HomeViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}", UriKind.RelativeOrAbsolute),
                    Title = ContactUsPageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HomeBodyViewModel = new HomeBodyViewModel(),
            };

            var sharedhtml = await sharedContentRedis.GetDataAsync<SharedHtml>(Constants.ContactusSharedContent, status);

            viewModel.HomeBodyViewModel.ContactUs = sharedhtml.Html;

            Logger.LogWarning($"{nameof(HomeView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/home")]
        public async Task<IActionResult> HomeView(HomeBodyViewModel? model)
        {
            if (model != null && ModelState.IsValid)
            {
                switch (model.SelectedOption)
                {
                    case HomeOption.Webchat:
                        return Redirect($"/{LocalPath}/{ChatController.ThisViewCanonicalName}");
                    case HomeOption.SendAMessage:
                        if (await SetSessionStateAsync(Category.None, null, false).ConfigureAwait(false))
                        {
                            return Redirect($"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}");
                        }

                        break;
                    case HomeOption.Callback:
                        if (await SetSessionStateAsync(Category.None, null, true).ConfigureAwait(false))
                        {
                            return Redirect($"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}");
                        }

                        break;
                    case HomeOption.Sendletter:
                        return Redirect($"/{LocalPath}/{SendUsLetterCanonicalName}");
                }
            }

            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Contact us",
            };
            var viewModel = new HomeViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}", UriKind.RelativeOrAbsolute),
                    Title = ContactUsPageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HomeBodyViewModel = model,
            };

            Logger.LogWarning($"{nameof(HomeView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home/head")]
        [Route("pages/head")]
        public IActionResult HomeHead()
        {
            var viewModel = new HeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}", UriKind.RelativeOrAbsolute),
                Title = ContactUsPageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(HomeHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/home/breadcrumb")]
        [Route("pages/breadcrumb")]
        public IActionResult HomeBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Contact us",
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(HomeBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public async Task<IActionResult> HomeBody()
        {
            await DeleteSessionStateAsync().ConfigureAwait(false);

            var viewModel = new HomeBodyViewModel();

            var sharedhtml = await sharedContentRedis.GetDataAsync<SharedHtml>(Constants.ContactusSharedContent, status);

            viewModel.ContactUs = sharedhtml.Html;

            Logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public async Task<IActionResult> HomeBody(HomeBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedOption)
                {
                    case HomeOption.Webchat:
                        return Redirect($"/{WebchatRegistrationPath}/{ChatController.ThisViewCanonicalName}");
                    case HomeOption.SendAMessage:
                        if (await SetSessionStateAsync(Category.None, null, false).ConfigureAwait(false))
                        {
                            return Redirect($"/{RegistrationPath}/{HowCanWeHelpController.ThisViewCanonicalName}");
                        }

                        break;
                    case HomeOption.Callback:
                        if (await SetSessionStateAsync(Category.None, null, true).ConfigureAwait(false))
                        {
                            return Redirect($"/{RegistrationPath}/{HowCanWeHelpController.ThisViewCanonicalName}");
                        }

                        break;
                    case HomeOption.Sendletter:
                        return Redirect($"/{RegistrationPath}/{SendUsLetterCanonicalName}");
                }
            }

            viewModel = new HomeBodyViewModel();

            Logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
