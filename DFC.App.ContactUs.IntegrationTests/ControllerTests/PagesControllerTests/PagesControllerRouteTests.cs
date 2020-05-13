using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.IntegrationTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Integration")]
    public class PagesControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.ContactUs.Startup>>
    {
        private readonly CustomWebApplicationFactory<DFC.App.ContactUs.Startup> factory;

        public PagesControllerRouteTests(CustomWebApplicationFactory<DFC.App.ContactUs.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticles(factory);
        }

        public static IEnumerable<object[]> PagesContentRouteData => new List<object[]>
        {
            new object[] { "/" },
            new object[] { "/pages" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/htmlhead" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/breadcrumb" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/body" },
            new object[] { "/pages/htmlhead" },
            new object[] { "/pages/breadcrumb" },
            new object[] { "/pages/body" },
            new object[] { "/pages/home" },
            new object[] { "/pages/home/htmlhead" },
            new object[] { "/pages/home/breadcrumb" },
            new object[] { "/pages/home/body" },
            new object[] { "/pages/chat" },
            new object[] { "/pages/chat/htmlhead" },
            new object[] { "/pages/chat/breadcrumb" },
            new object[] { "/pages/chat/body" },
            new object[] { "/pages/why-do-you-want-to-contact-us" },
            new object[] { "/pages/why-do-you-want-to-contact-us/htmlhead" },
            new object[] { "/pages/why-do-you-want-to-contact-us/breadcrumb" },
            new object[] { "/pages/why-do-you-want-to-contact-us/body" },
        };

        public static IEnumerable<object[]> PagesNoContentRouteData => new List<object[]>
        {
            new object[] { $"/pages/bodytop" },
            new object[] { $"/pages/herobanner" },
            new object[] { $"/pages/sidebarright" },
            new object[] { $"/pages/sidebarleft" },
            new object[] { $"/pages/bodyfooter" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/bodytop" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/herobanner" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/sidebarright" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/sidebarleft" },
            new object[] { $"/pages/{DataSeeding.SendUsLetterArticleName}/bodyfooter" },
            new object[] { $"/pages/bodytop" },
            new object[] { $"/pages/herobanner" },
            new object[] { $"/pages/sidebarright" },
            new object[] { $"/pages/sidebarleft" },
            new object[] { $"/pages/bodyfooter" },
            new object[] { $"/pages/{DataSeeding.AlternativeArticleName}/bodytop" },
            new object[] { $"/pages/{DataSeeding.AlternativeArticleName}/herobanner" },
            new object[] { $"/pages/{DataSeeding.AlternativeArticleName}/sidebarright" },
            new object[] { $"/pages/{DataSeeding.AlternativeArticleName}/sidebarleft" },
            new object[] { $"/pages/{DataSeeding.AlternativeArticleName}/bodyfooter" },
        };

        [Theory]
        [MemberData(nameof(PagesContentRouteData))]
        public async Task GetPagesHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Html));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(PagesContentRouteData))]
        public async Task GetPagesJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Application.Json}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(PagesNoContentRouteData))]
        public async Task GetPagesEndpointsReturnSuccessAndNoContent(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(Skip = "Need to supply anti-forgery token - see here for details")]
        public async Task DeletePagesEndpointsReturnNotFound()
        {
            // Arrange
            var uri = new Uri($"/pages/{Guid.NewGuid()}", UriKind.Relative);
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            //TODO: idc: Need to supply anti-forgery token for DELETE

            // Act
            var response = await client.DeleteAsync(uri).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}