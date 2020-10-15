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
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class PagesController : BasePagesController<PagesController>
    {
        private readonly IDocumentService<EmailModel> emailDocumentService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(ILogger<PagesController> logger, ISessionStateService<SessionDataModel> sessionStateService, IDocumentService<EmailModel> emailDocumentService, AutoMapper.IMapper mapper) : base(logger, sessionStateService)
        {
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
            var emailModels = await emailDocumentService.GetAllAsync().ConfigureAwait(false);

            if (emailModels != null)
            {
                var documents = from a in emailModels.OrderBy(o => o.Title)
                                select mapper.Map<IndexDocumentViewModel>(a);

                viewModel.Documents.AddRange(documents);

                Logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                Logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{documentId}/document")]
        public async Task<IActionResult> Document(Guid documentId)
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

            Logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");

            return NoContent();
        }
    }
}