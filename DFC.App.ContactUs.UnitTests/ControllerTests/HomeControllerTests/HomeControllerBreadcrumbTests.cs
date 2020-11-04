using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerBreadcrumbTests : BaseHomeControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HomeControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeBreadcrumb();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

<<<<<<< HEAD
            model.Paths?.Count.Should().BeGreaterThan(0);
=======
            model.Breadcrumbs?.Count.Should().BeGreaterThan(0);
>>>>>>> story/DFCC-1169-refresh-nugets

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void HomeControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeBreadcrumb();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

<<<<<<< HEAD
            model.Paths?.Count.Should().BeGreaterThan(0);
=======
            model.Breadcrumbs?.Count.Should().BeGreaterThan(0);
>>>>>>> story/DFCC-1169-refresh-nugets

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HomeControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeBreadcrumb();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
