<<<<<<< HEAD
﻿using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
=======
﻿using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
>>>>>>> story/DFCC-1169-refresh-nugets

namespace DFC.App.ContactUs.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";

        private readonly ILogger<SitemapController> logger;
<<<<<<< HEAD
        private readonly IContentPageService<ContentPageModel> contentPageService;

        public SitemapController(ILogger<SitemapController> logger, IContentPageService<ContentPageModel> contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
=======

        public SitemapController(ILogger<SitemapController> logger)
        {
            this.logger = logger;
>>>>>>> story/DFCC-1169-refresh-nugets
        }

        [HttpGet]
        [Route("pages/sitemap")]
<<<<<<< HEAD
        public async Task<IActionResult> SitemapView()
        {
            var result = await Sitemap().ConfigureAwait(false);
=======
        public IActionResult SitemapView()
        {
            var result = Sitemap();
>>>>>>> story/DFCC-1169-refresh-nugets

            return result;
        }

        [HttpGet]
        [Route("/sitemap.xml")]
<<<<<<< HEAD
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
=======
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
>>>>>>> story/DFCC-1169-refresh-nugets
        }
    }
}