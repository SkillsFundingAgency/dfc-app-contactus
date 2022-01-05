using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HowCanWeHelpControllerTests
{
    [Trait("Category", "HowCanWeHelp Controller Unit Tests")]
    public class HowCanWeHelpControllerHeadTests : BaseHowCanWeHelpControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HowCanWeHelpControllerHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpHead();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void HowCanWeHelpControllerHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpHead();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HowCanWeHelpControllerHeadReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpHead();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
