using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    public abstract class BasePagesControllerTests
    {
        protected BasePagesControllerTests()
        {
            Logger = A.Fake<ILogger<PagesController>>();
            FakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
<<<<<<< HEAD
            FakeContentPageService = A.Fake<IContentPageService<ContentPageModel>>();
=======
            FakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();
>>>>>>> story/DFCC-1169-refresh-nugets
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

        protected ILogger<PagesController> Logger { get; }

        protected ISessionStateService<SessionDataModel> FakeSessionStateService { get; }

<<<<<<< HEAD
        protected IContentPageService<ContentPageModel> FakeContentPageService { get; }
=======
        protected IDocumentService<EmailModel> FakeEmailDocumentService { get; }
>>>>>>> story/DFCC-1169-refresh-nugets

        protected AutoMapper.IMapper FakeMapper { get; }

        protected PagesController BuildPagesController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

<<<<<<< HEAD
            var controller = new PagesController(Logger, FakeSessionStateService, FakeContentPageService, FakeMapper)
=======
            var controller = new PagesController(Logger, FakeSessionStateService, FakeEmailDocumentService, FakeMapper)
>>>>>>> story/DFCC-1169-refresh-nugets
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
