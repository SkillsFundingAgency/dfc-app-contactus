using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerHeadTests : BaseEnterYourDetailsControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void EnterYourDetailsControllerHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsHead();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void EnterYourDetailsControllerHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsHead();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void EnterYourDetailsControllerHeadReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsHead();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
