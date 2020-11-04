using DFC.App.ContactUs.Controllers;
<<<<<<< HEAD
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
=======
>>>>>>> story/DFCC-1169-refresh-nugets
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.SitemapControllerTests
{
    public class BaseSitemapControllerTests
    {
        public BaseSitemapControllerTests()
        {
            FakeLogger = A.Fake<ILogger<SitemapController>>();
<<<<<<< HEAD
            FakeContentPageService = A.Fake<IContentPageService<ContentPageModel>>();
=======
>>>>>>> story/DFCC-1169-refresh-nugets
        }

        protected ILogger<SitemapController> FakeLogger { get; }

<<<<<<< HEAD
        protected IContentPageService<ContentPageModel> FakeContentPageService { get; }

        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(FakeLogger, FakeContentPageService)
=======
        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(FakeLogger)
>>>>>>> story/DFCC-1169-refresh-nugets
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
