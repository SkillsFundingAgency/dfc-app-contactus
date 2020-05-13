using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Controllers
{
    public class ChatController : BasePagesController<ChatController>
    {
        public ChatController(ILogger<ChatController> logger) : base(logger)
        {
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
                Title = "Webchat | Contact us | National Careers Service",
            };

            Logger.LogInformation($"{nameof(ChatHtmlHead)} has returned content");

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

            Logger.LogInformation($"{nameof(ChatBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/chat/body")]
        public IActionResult ChatBody()
        {
            Logger.LogInformation($"{nameof(ChatBody)} has returned content");

            return this.NegotiateContentResult(default, new object());
        }
    }
}
