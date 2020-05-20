﻿using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.PageService;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class PagesController : BasePagesController<PagesController>
    {
        private readonly IContentPageService contentPageService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(ILogger<PagesController> logger, IContentPageService contentPageService, AutoMapper.IMapper mapper) : base(logger)
        {
            this.contentPageService = contentPageService;
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
            };
            var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            if (contentPageModels != null)
            {
                viewModel.Documents = (from a in contentPageModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = "health" });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = HomeController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = ChatController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = WhyContactUsController.ThisViewCanonicalName });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = EnterYourDetailsController.ThisViewCanonicalName });

                Logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                Logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
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

                return this.NegotiateContentResult(viewModel);
            }

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

        [HttpPost]
        [Route("pages")]
        public async Task<IActionResult> Create([FromBody]ContentPageModel? upsertContentPageModel)
        {
            if (upsertContentPageModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            Logger.LogInformation($"{nameof(Create)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("pages")]
        public async Task<IActionResult> Update([FromBody]ContentPageModel? upsertContentPageModel)
        {
            if (upsertContentPageModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (upsertContentPageModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            upsertContentPageModel.Etag = existingDocument.Etag;

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            Logger.LogInformation($"{nameof(Update)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            var isDeleted = await contentPageService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                Logger.LogInformation($"{nameof(Delete)} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                Logger.LogWarning($"{nameof(Delete)} has returned no content for: {documentId}");
                return NotFound();
            }
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
            var contentPageModel = await contentPageService.GetByAlternativeNameAsync(article).ConfigureAwait(false);

            return contentPageModel;
        }

        #endregion Define helper methods
    }
}