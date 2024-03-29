﻿using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Controllers
{
    public class PagesController : BasePagesController<PagesController>
    {
        public PagesController(
            ILogger<PagesController> logger,
            ISessionStateService<SessionDataModel> sessionStateService) : base(logger, sessionStateService)
        {
        }

        [HttpGet]
        [Route("/")]
        [Route("pages")]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                LocalPath = LocalPath,
                Documents = new List<IndexDocumentViewModel>()
                {
                    new IndexDocumentViewModel { Title = HealthController.HealthViewCanonicalName },
                    new IndexDocumentViewModel { Title = SitemapController.SitemapViewCanonicalName },
                    new IndexDocumentViewModel { Title = RobotController.RobotsViewCanonicalName },
                    new IndexDocumentViewModel { Title = HomeController.ThisViewCanonicalName },
                    new IndexDocumentViewModel { Title = ChatController.ThisViewCanonicalName },
                    new IndexDocumentViewModel { Title = HowCanWeHelpController.ThisViewCanonicalName },
                    new IndexDocumentViewModel { Title = EnterYourDetailsController.ThisViewCanonicalName },
                },
            };

            Logger.LogInformation($"{nameof(Index)} has succeeded");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/document")]
        public IActionResult Document()
        {
            var viewModel = new DocumentViewModel();
            var breadcrumbItemModel = new BreadcrumbItemModel();

            viewModel.Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(Document)} has succeeded");

            return this.NegotiateContentResult(viewModel);
        }
    }
}