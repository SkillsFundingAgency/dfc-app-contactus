using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Category", "Health Controller Unit Tests")]
    public class HealthControllerViewTests : BaseHealthControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HealthControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = controller.HealthView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HealthViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void HealthControllerViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = controller.HealthView();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<IList<HealthItemViewModel>>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HealthControllerHealthViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHealthController(mediaTypeName);

            // Act
            var result = controller.HealthView();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
