using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerBreadcrumbTests : BaseEnterYourDetailsController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void EnterYourDetailsControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBreadcrumb();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void EnterYourDetailsControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBreadcrumb();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void EnterYourDetailsControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBreadcrumb();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
