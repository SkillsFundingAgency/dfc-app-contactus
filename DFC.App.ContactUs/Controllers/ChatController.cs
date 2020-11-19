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
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class ChatController : BasePagesController<ChatController>
    {
        public const string ThisViewCanonicalName = "chat";
        private readonly ChatOptions chatOptions;
        private readonly AutoMapper.IMapper mapper;

        private readonly IDocumentService<ConfigurationSetModel> configurationSetDocumentService;

        public ChatController(ILogger<ChatController> logger, ISessionStateService<SessionDataModel> sessionStateService, ChatOptions chatOptions, AutoMapper.IMapper mapper, IDocumentService<ConfigurationSetModel> configurationSetDocumentService) : base(logger, sessionStateService)
        {
            this.chatOptions = chatOptions;
            this.mapper = mapper;
            this.configurationSetDocumentService = configurationSetDocumentService;
        }

        [HttpGet]
        [Route("pages/chat")]
        public async Task<IActionResult> ChatView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Chat",
            };
            var viewModel = new ChatViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/chat", UriKind.RelativeOrAbsolute),
                    Title = "Webchat" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                ChatViewBodyModel = mapper.Map<ChatViewBodyModel>(chatOptions),
            };

            var configurationSetModel = await configurationSetDocumentService.GetByIdAsync(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false) ?? new ConfigurationSetModel();
            viewModel.ChatViewBodyModel.PhoneNumber = configurationSetModel?.PhoneNumber ?? ConfigurationSetModel.DefaultPhoneNumber;

            Logger.LogWarning($"{nameof(ChatView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/htmlhead")]
        public IActionResult ChatHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{WebchatRegistrationPath}/chat", UriKind.RelativeOrAbsolute),
                Title = "Webchat" + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(ChatHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/chat/breadcrumb")]
        public IActionResult ChatBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Chat",
            };
            var viewModel = BuildBreadcrumb(WebchatRegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(ChatBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/body")]
        public async Task<IActionResult> ChatBody()
        {
            var viewModel = mapper.Map<ChatViewBodyModel>(chatOptions);

            var configurationSetModel = await configurationSetDocumentService.GetByIdAsync(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false) ?? new ConfigurationSetModel();
            viewModel.PhoneNumber = configurationSetModel?.PhoneNumber ?? ConfigurationSetModel.DefaultPhoneNumber;

            Logger.LogInformation($"{nameof(ChatBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}
