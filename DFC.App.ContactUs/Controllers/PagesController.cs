using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class PagesController : BasePagesController<PagesController>
    {
        private readonly IDocumentService<ConfigurationSetModel> configurationSetDocumentService;
        private readonly IDocumentService<EmailModel> emailDocumentService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(
            ILogger<PagesController> logger,
            ISessionStateService<SessionDataModel> sessionStateService,
            IDocumentService<ConfigurationSetModel> configurationSetDocumentService,
            IDocumentService<EmailModel> emailDocumentService,
            AutoMapper.IMapper mapper) : base(logger, sessionStateService)
        {
            this.configurationSetDocumentService = configurationSetDocumentService;
            this.emailDocumentService = emailDocumentService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/")]
        [Route("pages")]
        public async Task<IActionResult> Index()
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

            var configurationSetModel = await configurationSetDocumentService.GetByIdAsync(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false);

            if (configurationSetModel != null)
            {
                viewModel.Documents.Add(mapper.Map<IndexDocumentViewModel>(configurationSetModel));
            }

            foreach (var key in EmailKeyHelper.GetEmailKeys())
            {
                var emailModel = await emailDocumentService.GetByIdAsync(key).ConfigureAwait(false);

                if (emailModel != null)
                {
                    viewModel.Documents.Add(mapper.Map<IndexDocumentViewModel>(emailModel));
                }
            }

            Logger.LogInformation($"{nameof(Index)} has succeeded");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{documentId}/document")]
        public async Task<IActionResult> Document(Guid documentId)
        {
            if (documentId.Equals(ConfigurationSetKeyHelper.ConfigurationSetKey))
            {
                var configurationSetModel = await configurationSetDocumentService.GetByIdAsync(documentId).ConfigureAwait(false);

                if (configurationSetModel != null)
                {
                    var viewModel = mapper.Map<DocumentViewModel>(configurationSetModel);
                    var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(configurationSetModel);

                    viewModel.Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel);

                    Logger.LogInformation($"{nameof(Document)} has succeeded for: {documentId}");

                    return this.NegotiateContentResult(viewModel);
                }
            }
            else
            {
                var emailModel = await emailDocumentService.GetByIdAsync(documentId).ConfigureAwait(false);

                if (emailModel != null)
                {
                    var viewModel = mapper.Map<DocumentViewModel>(emailModel);
                    var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(emailModel);

                    viewModel.Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel);

                    Logger.LogInformation($"{nameof(Document)} has succeeded for: {documentId}");

                    return this.NegotiateContentResult(viewModel);
                }
            }

            Logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");

            return NoContent();
        }
    }
}