using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.PageService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> logger;
        private readonly IContentPageService contentPageService;

        public SitemapController(ILogger<SitemapController> logger, IContentPageService contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
        }

        [HttpGet]
        [Route("/sitemap.xml")]
        public async Task<IActionResult> Sitemap()
        {
            try
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

                var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

                if (contentPageModels != null)
                {
                    var contentPageModelsList = contentPageModels.ToList();

                    if (contentPageModelsList.Any())
                    {
                        var sitemapContentPageModels = contentPageModelsList
                             .Where(w => w.IncludeInSitemap)
                             .OrderBy(o => o.CanonicalName);

                        foreach (var contentPageModel in sitemapContentPageModels)
                        {
                            sitemap.Add(new SitemapLocation
                            {
                                Url = $"{sitemapUrlPrefix}/{contentPageModel.CanonicalName}",
                                Priority = 1,
                            });
                        }
                    }
                }

                if (!sitemap.Locations.Any())
                {
                    return NoContent();
                }

                var xmlString = sitemap.WriteSitemapToString();

                logger.LogInformation("Generated Sitemap");

                return Content(xmlString, MediaTypeNames.Application.Xml);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Sitemap)}: {ex.Message}");
            }

            return BadRequest();
        }
    }
}