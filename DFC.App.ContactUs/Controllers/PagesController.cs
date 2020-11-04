using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
<<<<<<< HEAD
using System.Linq;
using System.Net;
=======
using System.Collections.Generic;
using System.Linq;
>>>>>>> story/DFCC-1169-refresh-nugets
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class PagesController : BasePagesController<PagesController>
    {
<<<<<<< HEAD
        private readonly IContentPageService<ContentPageModel> contentPageService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(ILogger<PagesController> logger, ISessionStateService<SessionDataModel> sessionStateService, IContentPageService<ContentPageModel> contentPageService, AutoMapper.IMapper mapper) : base(logger, sessionStateService)
        {
            this.contentPageService = contentPageService;
=======
        private readonly IDocumentService<EmailModel> emailDocumentService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(ILogger<PagesController> logger, ISessionStateService<SessionDataModel> sessionStateService, IDocumentService<EmailModel> emailDocumentService, AutoMapper.IMapper mapper) : base(logger, sessionStateService)
        {
            this.emailDocumentService = emailDocumentService;
>>>>>>> story/DFCC-1169-refresh-nugets
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
<<<<<<< HEAD
            };
            var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            if (contentPageModels != null)
            {
                viewModel.Documents = (from a in contentPageModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = HealthController.HealthViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = SitemapController.SitemapViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = HomeController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = ChatController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = HowCanWeHelpController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = EnterYourDetailsController.ThisViewCanonicalName });
=======
                Documents = new List<IndexDocumentViewModel>()
                {
                    new IndexDocumentViewModel { Title = HealthController.HealthViewCanonicalName },
                    new IndexDocumentViewModel { Title = SitemapController.SitemapViewCanonicalName },
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
>>>>>>> story/DFCC-1169-refresh-nugets

                Logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                Logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
<<<<<<< HEAD
        [Route("pages/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(contentPageModel);
                var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(contentPageModel);

                viewModel.HtmlHead = mapper.Map<HtmlHeadViewModel>(contentPageModel);
                viewModel.Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel);

                Logger.LogInformation($"{nameof(Document)} has succeeded for: {article}");
=======
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
>>>>>>> story/DFCC-1169-refresh-nugets

                return this.NegotiateContentResult(viewModel);
            }

<<<<<<< HEAD
            if (!string.IsNullOrWhiteSpace(article))
            {
                var alternateContentPageModel = await GetAlternativeContentPageAsync(article).ConfigureAwait(false);

                if (alternateContentPageModel != null)
                {
                    var alternateUrl = $"{Request.GetBaseAddress()}{LocalPath}/{alternateContentPageModel.CanonicalName}";
                    Logger.LogWarning($"{nameof(Document)} has been redirected for: {article} to {alternateUrl}");

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            Logger.LogWarning($"{nameof(Document)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        public async Task<IActionResult> HtmlHead(string? article)
        {
            var viewModel = new HtmlHeadViewModel();
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                mapper.Map(contentPageModel, viewModel);

                viewModel.CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/{contentPageModel.CanonicalName}", UriKind.RelativeOrAbsolute);
            }

            Logger.LogInformation($"{nameof(HtmlHead)} has returned content for: {article}");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/{article}/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string? article)
        {
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);
            var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(contentPageModel);
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(Breadcrumb)} has returned content for: {article}");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}/bodytop")]
        [Route("pages/bodytop")]
        public IActionResult BodyTop(string? article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/herobanner")]
        [Route("pages/herobanner")]
        public IActionResult HeroBanner(string? article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/body")]
        public async Task<IActionResult> Body(string? article)
        {
            var viewModel = new BodyViewModel();
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                mapper.Map(contentPageModel, viewModel);
                Logger.LogInformation($"{nameof(Body)} has returned content for: {article}");

                return this.NegotiateContentResult(viewModel, contentPageModel);
            }

            if (!string.IsNullOrWhiteSpace(article))
            {
                var alternateContentPageModel = await GetAlternativeContentPageAsync(article).ConfigureAwait(false);

                if (alternateContentPageModel != null)
                {
                    var alternateUrl = $"{Request.GetBaseAddress()}{RegistrationPath}/{alternateContentPageModel.CanonicalName}";
                    Logger.LogWarning($"{nameof(Body)} has been redirected for: {article} to {alternateUrl}");

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            Logger.LogWarning($"{nameof(Body)} has not returned any content for: {article}");
            return NotFound();
        }

        [HttpGet]
        [Route("pages/{article}/sidebarright")]
        [Route("pages/sidebarright")]
        public IActionResult SidebarRight(string? article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/sidebarleft")]
        [Route("pages/sidebarleft")]
        public IActionResult SidebarLeft(string? article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/bodyfooter")]
        [Route("pages/bodyfooter")]
        public IActionResult BodyFooter(string? article)
        {
            return NoContent();
        }

        #region Define helper methods

        private async Task<ContentPageModel?> GetContentPageAsync(string? article)
        {
            const string defaultArticleName = HomeController.ThisViewCanonicalName;
            var articleName = string.IsNullOrWhiteSpace(article) ? defaultArticleName : article;

            var contentPageModel = await contentPageService.GetByNameAsync(articleName).ConfigureAwait(false);

            return contentPageModel;
        }

        private async Task<ContentPageModel?> GetAlternativeContentPageAsync(string article)
        {
            var contentPageModel = await contentPageService.GetByRedirectLocationAsync(article).ConfigureAwait(false);

            return contentPageModel;
        }

        #endregion Define helper methods
=======
            Logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");

            return NoContent();
        }
>>>>>>> story/DFCC-1169-refresh-nugets
    }
}