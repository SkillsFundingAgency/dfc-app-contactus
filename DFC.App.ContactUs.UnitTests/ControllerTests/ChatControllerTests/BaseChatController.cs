﻿using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    public abstract class BaseChatController
    {
        protected BaseChatController()
        {
            Logger = A.Fake<ILogger<ChatController>>();
            ChatOptions = new ChatOptions { ChatUrl = new System.Uri("https://somewhere.com/webchat") };
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*/*" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected ILogger<ChatController> Logger { get; }

        protected ChatOptions ChatOptions { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected ChatController BuildChatController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new ChatController(Logger, ChatOptions, FakeMapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
