using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.ContactUs.IntegrationTests.ControllerTests
{
    public static class DataSeeding
    {
        public const string DefaultArticleName = "home";
        public const string AlternativeArticleName = "alternative-name";

        public static void SeedDefaultArticles(CustomWebApplicationFactory<DFC.App.ContactUs.Startup> factory)
        {
            const string url = "/pages";
            var contentPageModels = new List<ContentPageModel>()
            {
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("5DDE75FF-8B32-4746-9712-2672E5C540DB"),
                    CanonicalName = DefaultArticleName,
                    BreadcrumbTitle = "Contact Us",
                    IncludeInSitemap = true,
                    Version = Guid.NewGuid(),
                    Url = new Uri("https://localhost"),
                    Content = "<h1>A document</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("9244BFF6-BA0C-40DB-AD52-A293C37441B1"),
                    CanonicalName = "in-sitemap",
                    BreadcrumbTitle = "In Sitemap",
                    IncludeInSitemap = true,
                    Version = Guid.NewGuid(),
                    Url = new Uri("https://localhost"),
                    Content = "<h1>A document</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-BF66-1B2DB192177E"),
                    CanonicalName = "not-in-sitemap",
                    BreadcrumbTitle = "Not in Sitemap",
                    IncludeInSitemap = false,
                    Version = Guid.NewGuid(),
                    Url = new Uri("https://localhost"),
                    Content = "<h1>A document</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("DA9C1643-8937-4D09-843C-102E15CA3D1B"),
                    CanonicalName = "contains-alternative-name",
                    BreadcrumbTitle = "Contains Alternative Name",
                    IncludeInSitemap = false,
                    Version = Guid.NewGuid(),
                    Url = new Uri("https://localhost"),
                    AlternativeNames = new[] { AlternativeArticleName },
                    Content = "<h1>A document</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
            };

            var client = factory?.CreateClient();

            client?.DefaultRequestHeaders.Accept.Clear();

            contentPageModels.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()).GetAwaiter().GetResult());
        }
    }
}
