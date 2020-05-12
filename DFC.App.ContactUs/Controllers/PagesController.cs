using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.PageService;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PagesController : Controller
    {
        public const string RegistrationPath = "contact-us";
        public const string WebchatRegistrationPath = "webchat";
        public const string LocalPath = "pages";

        private readonly ILogger<PagesController> logger;
        private readonly IContentPageService contentPageService;
        private readonly AutoMapper.IMapper mapper;
        private readonly ServiceOpenDetailModel serviceOpenDetailModel;

        public PagesController(ILogger<PagesController> logger, IContentPageService contentPageService, AutoMapper.IMapper mapper, ServiceOpenDetailModel serviceOpenDetailModel)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
            this.mapper = mapper;
            this.serviceOpenDetailModel = serviceOpenDetailModel;
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

                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = "home" });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = "chat" });
                viewModel.Documents.Add(new IndexDocumentViewModel { CanonicalName = "why-do-you-want-to-contact-us" });

                logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                logger.LogWarning($"{nameof(Index)} has returned with no results");
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

                logger.LogInformation($"{nameof(Document)} has succeeded for: {article}");

                return this.NegotiateContentResult(viewModel);
            }

            if (!string.IsNullOrWhiteSpace(article))
            {
                var alternateContentPageModel = await GetAlternativeContentPageAsync(article).ConfigureAwait(false);

                if (alternateContentPageModel != null)
                {
                    var alternateUrl = $"{Request.GetBaseAddress()}{LocalPath}/{alternateContentPageModel.CanonicalName}";
                    logger.LogWarning($"{nameof(Document)} has been redirected for: {article} to {alternateUrl}");

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            logger.LogWarning($"{nameof(Document)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/chat")]
        public IActionResult ChatView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "chat",
                BreadcrumbTitle = "Chat",
            };
            var viewModel = new ChatViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/chat", UriKind.RelativeOrAbsolute),
                    Title = "Webchat | Contact us | National Careers Service",
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
            };

            logger.LogWarning($"{nameof(ChatView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home")]
        public IActionResult HomeView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "home",
                BreadcrumbTitle = "contact us",
            };
            var viewModel = new HomeViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}", UriKind.RelativeOrAbsolute),
                    Title = "Contact us | National Careers Service",
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HomeBodyViewModel = new HomeBodyViewModel
                {
                    ServiceOpenDetailModel = serviceOpenDetailModel,
                },
            };

            logger.LogWarning($"{nameof(HomeView)} has returned content");

            return this.NegotiateContentResult(viewModel);
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
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/chat", UriKind.RelativeOrAbsolute),
                    Title = "Why do you want to contact us | Contact us | National Careers Service",
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                WhyContactUsBodyViewModel = new WhyContactUsBodyViewModel(),
            };

            logger.LogWarning($"{nameof(WhyContactUsView)} has returned content");

            return this.NegotiateContentResult(viewModel);
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

            logger.LogInformation($"{nameof(HtmlHead)} has returned content for: {article}");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/htmlhead")]
        public IActionResult ChatHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{WebchatRegistrationPath}/chat", UriKind.RelativeOrAbsolute),
                Title = "Webchat | Contact us | National Careers Service",
            };

            logger.LogInformation($"{nameof(ChatHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home/htmlhead")]
        [Route("pages/htmlhead")]
        public IActionResult HomeHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}", UriKind.RelativeOrAbsolute),
                Title = "Contact us | National Careers Service",
            };

            logger.LogInformation($"{nameof(HomeHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us/htmlhead")]
        public IActionResult WhyContactUsHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}/chat", UriKind.RelativeOrAbsolute),
                Title = "Why do you want to contact us | Contact us | National Careers Service",
            };

            logger.LogInformation($"{nameof(WhyContactUsHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/{article}/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string? article)
        {
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);
            var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(contentPageModel);
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            logger.LogInformation($"{nameof(Breadcrumb)} has returned content for: {article}");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/chat/breadcrumb")]
        public IActionResult ChatBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "chat",
                BreadcrumbTitle = "Chat",
            };
            var viewModel = BuildBreadcrumb(WebchatRegistrationPath, breadcrumbItemModel);

            logger.LogInformation($"{nameof(ChatBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/home/breadcrumb")]
        [Route("pages/breadcrumb")]
        public IActionResult HomeBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "home",
                BreadcrumbTitle = "Contact us",
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            logger.LogInformation($"{nameof(HomeBreadcrumb)} has returned content");

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

            logger.LogInformation($"{nameof(WhyContactUsBreadcrumb)} has returned content");

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
                logger.LogInformation($"{nameof(Body)} has returned content for: {article}");

                return this.NegotiateContentResult(viewModel, contentPageModel);
            }

            if (!string.IsNullOrWhiteSpace(article))
            {
                var alternateContentPageModel = await GetAlternativeContentPageAsync(article).ConfigureAwait(false);

                if (alternateContentPageModel != null)
                {
                    var alternateUrl = $"{Request.GetBaseAddress()}{RegistrationPath}/{alternateContentPageModel.CanonicalName}";
                    logger.LogWarning($"{nameof(Body)} has been redirected for: {article} to {alternateUrl}");

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            logger.LogWarning($"{nameof(Body)} has not returned any content for: {article}");
            return NotFound();
        }

        [HttpGet]
        [Route("pages/chat/body")]
        public IActionResult ChatBody()
        {
            logger.LogInformation($"{nameof(ChatBody)} has returned content");

            return this.NegotiateContentResult(default, new object());
        }

        [HttpGet]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public IActionResult HomeBody()
        {
            var viewModel = new HomeBodyViewModel()
            {
                ServiceOpenDetailModel = serviceOpenDetailModel,
            };

            logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public IActionResult HomeBody(HomeBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedOption)
                {
                    case HomeBodyViewModel.SelectOption.Webchat:
                        return Redirect($"/{WebchatRegistrationPath}/chat");
                    case HomeBodyViewModel.SelectOption.SendAMessage:
                        return Redirect($"/{RegistrationPath}/why-do-you-want-to-contact-us");
                    case HomeBodyViewModel.SelectOption.Callback:
                        return Redirect($"/{RegistrationPath}/request-callback");
                    case HomeBodyViewModel.SelectOption.Sendletter:
                        return Redirect($"/{RegistrationPath}/send-us-a-letter");
                }
            }

            viewModel = new HomeBodyViewModel()
            {
                ServiceOpenDetailModel = serviceOpenDetailModel,
            };

            logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/why-do-you-want-to-contact-us/body")]
        public IActionResult WhyContactUsBody()
        {
            var viewModel = new WhyContactUsBodyViewModel();

            logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

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

            ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.MoreDetail), "eeeeeeeeekkkkkkkkkkkkkkkkk");

            logger.LogInformation($"{nameof(WhyContactUsBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
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

            logger.LogInformation($"{nameof(Create)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

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

            logger.LogInformation($"{nameof(Update)} has upserted content for: {upsertContentPageModel.CanonicalName} with response code {response}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            var isDeleted = await contentPageService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                logger.LogInformation($"{nameof(Delete)} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                logger.LogWarning($"{nameof(Delete)} has returned no content for: {documentId}");
                return NotFound();
            }
        }

        #region Define helper methods

        private static BreadcrumbViewModel BuildBreadcrumb(string segmentPath, BreadcrumbItemModel? breadcrumbItemModel)
        {
            const string BradcrumbTitle = "Contact us";
            var viewModel = new BreadcrumbViewModel
            {
                Paths = new List<BreadcrumbPathViewModel>()
                {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home",
                    },
                    new BreadcrumbPathViewModel()
                    {
                        Route = $"/{segmentPath}",
                        Title = BradcrumbTitle,
                    },
                },
            };

            if (breadcrumbItemModel?.BreadcrumbTitle != null &&
                !breadcrumbItemModel.BreadcrumbTitle.Equals(BradcrumbTitle, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(breadcrumbItemModel.CanonicalName))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{segmentPath}/{breadcrumbItemModel.CanonicalName}",
                    Title = breadcrumbItemModel.BreadcrumbTitle,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        private async Task<ContentPageModel?> GetContentPageAsync(string? article)
        {
            const string defaultArticleName = "home";
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