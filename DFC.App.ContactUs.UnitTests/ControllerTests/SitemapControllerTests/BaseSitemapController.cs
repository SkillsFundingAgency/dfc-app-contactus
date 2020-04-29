using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.PageService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.SitemapControllerTests
{
    public class BaseSitemapController
    {
        public BaseSitemapController()
        {
            FakeLogger = A.Fake<ILogger<SitemapController>>();
            FakeContentPageService = A.Fake<IContentPageService>();
        }

        protected ILogger<SitemapController> FakeLogger { get; }

        protected IContentPageService FakeContentPageService { get; }

        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(FakeLogger, FakeContentPageService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext(),
                },
            };

            return controller;
        }
    }
}
