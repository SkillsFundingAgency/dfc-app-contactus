using FakeItEasy;
using System;
using System.Collections.Generic;
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
        }

        public static IEnumerable<object[]> PagesContentRouteData => new List<object[]>
        {
            new object[] { "/" },
            new object[] { "/pages" },
            new object[] { "/pages/home" },
            new object[] { "/pages/home/htmlhead" },
            new object[] { "/pages/home/breadcrumb" },
            new object[] { "/pages/home/body" },
            new object[] { "/pages/chat" },
            new object[] { "/pages/chat/htmlhead" },
            new object[] { "/pages/chat/breadcrumb" },
            new object[] { "/pages/chat/body" },
            new object[] { "/pages/enter-your-details" },
            new object[] { "/pages/enter-your-details/htmlhead" },
            new object[] { "/pages/enter-your-details/breadcrumb" },
            new object[] { "/pages/enter-your-details/body" },
            new object[] { "/pages/how-can-we-help" },
            new object[] { "/pages/how-can-we-help/htmlhead" },
            new object[] { "/pages/how-can-we-help/breadcrumb" },
            new object[] { "/pages/how-can-we-help/body" },
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
            var response = await client.GetAsync(uri);

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
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Application.Json}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }
    }
}