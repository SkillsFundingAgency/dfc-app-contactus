﻿using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    public abstract class BaseHomeControllerTests
    {
        protected const string LocalPath = "pages";

        protected BaseHomeControllerTests()
        {
            Logger = A.Fake<ILogger<HomeController>>();
            FakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
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

        protected ILogger<HomeController> Logger { get; }

        protected ISessionStateService<SessionDataModel> FakeSessionStateService { get; }

        protected HomeController BuildHomeController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new HomeController(Logger, FakeSessionStateService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, Guid.NewGuid().ToString());

            return controller;
        }
    }
}
