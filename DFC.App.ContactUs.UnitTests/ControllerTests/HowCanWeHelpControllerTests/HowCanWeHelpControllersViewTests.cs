using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HowCanWeHelpControllerTests
{
    [Trait("Category", "HowCanWeHelp Controller Unit Tests")]
    public class HowCanWeHelpControllersViewTests : BaseHowCanWeHelpControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HowCanWeHelpControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HowCanWeHelpViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void HowCanWeHelpControllersViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpView();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<HowCanWeHelpViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HowCanWeHelpControllerViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = controller.HowCanWeHelpView();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
