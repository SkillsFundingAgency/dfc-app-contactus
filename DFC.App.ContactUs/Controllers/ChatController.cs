﻿using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
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

        public ChatController(ILogger<ChatController> logger, ChatOptions chatOptions, AutoMapper.IMapper mapper) : base(logger)
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
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Chat",
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
                CanonicalName = ThisViewCanonicalName,
                BreadcrumbTitle = "Chat",
            };
            var viewModel = BuildBreadcrumb(WebchatRegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(ChatBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/body")]
        public IActionResult ChatBody()
        {
            var viewModel = mapper.Map<ChatViewBodyModel>(chatOptions);

            Logger.LogInformation($"{nameof(ChatBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }
    }
}
