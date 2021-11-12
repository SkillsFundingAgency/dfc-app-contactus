using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Category", "Health Controller Unit Tests")]
    public class HealthControllerViewTests : BaseHealthControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task HealthControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = await controller.HealthView().ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HealthViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HealthControllerViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = await controller.HealthView().ConfigureAwait(false);

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<IList<HealthItemViewModel>>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HealthControllerHealthViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = await controller.HealthView().ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
