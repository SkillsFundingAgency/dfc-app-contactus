using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerBodyTests : BaseEnterYourDetailsControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void EnterYourDetailsControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBody();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void EnterYourDetailsControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBody();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void EnterYourDetailsControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBody();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
