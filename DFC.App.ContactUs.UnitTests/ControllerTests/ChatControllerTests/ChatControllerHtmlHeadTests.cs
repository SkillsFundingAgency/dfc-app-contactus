using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    [Trait("Category", "Chat Controller Unit Tests")]
    public class ChatControllerHtmlHeadTests : BaseChatController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void ChatControllerHtmlHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatHtmlHead();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HtmlHeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void ChatControllerHtmlHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatHtmlHead();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HtmlHeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void ChatControllerHtmlHeadReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatHtmlHead();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
