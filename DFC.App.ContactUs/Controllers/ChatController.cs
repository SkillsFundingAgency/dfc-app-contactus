using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Controllers
{
    public class ChatController : BasePagesController<ChatController>
    {
        public const string ThisViewCanonicalName = "chat";
        private readonly ChatOptions chatOptions;
        private readonly AutoMapper.IMapper mapper;

        public ChatController(ILogger<ChatController> logger, ISessionStateService<SessionDataModel> sessionStateService, ChatOptions chatOptions, AutoMapper.IMapper mapper) : base(logger, sessionStateService)
        {
            this.chatOptions = chatOptions;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("pages/chat")]
        public IActionResult ChatView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                Route = ThisViewCanonicalName,
                Title = "Chat",
            };
            var viewModel = new ChatViewModel()
            {
                Head = new HeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}/chat", UriKind.RelativeOrAbsolute),
                    Title = "Webchat" + PageTitleSuffix,
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                ChatViewBodyModel = mapper.Map<ChatViewBodyModel>(chatOptions),
            };

            viewModel.ChatViewBodyModel.HowCanWeHelpLink = $"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}";

            Logger.LogWarning($"{nameof(ChatView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/head")]
        public IActionResult ChatHead()
        {
            var viewModel = new HeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{WebchatRegistrationPath}/chat", UriKind.RelativeOrAbsolute),
                Title = "Webchat" + PageTitleSuffix,
            };

            Logger.LogInformation($"{nameof(ChatHead)} has returned content");

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
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(ChatBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/body")]
        public IActionResult ChatBody()
        {
            var viewModel = mapper.Map<ChatViewBodyModel>(chatOptions);

            viewModel.HowCanWeHelpLink = $"/{RegistrationPath}/{HowCanWeHelpController.ThisViewCanonicalName}";

            Logger.LogInformation($"{nameof(ChatBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}
