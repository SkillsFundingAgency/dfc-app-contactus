using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerWhyContactUsBreadcrumbTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerWhyContactUsBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBreadcrumb();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerWhyContactUsBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBreadcrumb();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerWhyContactUsBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBreadcrumb();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
