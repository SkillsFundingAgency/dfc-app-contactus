using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;

namespace DFC.App.ContactUs.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";

        private readonly ILogger<SitemapController> logger;

        public SitemapController(ILogger<SitemapController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("pages/sitemap")]
        public IActionResult SitemapView()
        {
            var result = Sitemap();

            return result;
        }

        [HttpGet]
        [Route("/sitemap.xml")]
        public IActionResult Sitemap()
        {
            logger.LogInformation("Generating Sitemap");

            var sitemapUrlPrefix = $"{Request.GetBaseAddress()}{BasePagesController<PagesController>.RegistrationPath}";
            var sitemap = new Sitemap();

            // add the defaults
            sitemap.Add(new SitemapLocation
            {
                Url = sitemapUrlPrefix,
                Priority = 1,
            });

            if (!sitemap.Locations.Any())
            {
                return NoContent();
            }

            var xmlString = sitemap.WriteSitemapToString();

            logger.LogInformation("Generated Sitemap");

            return Content(xmlString, MediaTypeNames.Application.Xml);
        }
    }
}