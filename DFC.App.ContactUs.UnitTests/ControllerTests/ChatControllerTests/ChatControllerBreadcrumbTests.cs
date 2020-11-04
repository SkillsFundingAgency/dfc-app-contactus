using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    [Trait("Category", "Chat Controller Unit Tests")]
    public class ChatControllerBreadcrumbTests : BaseChatControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void ChatControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatBreadcrumb();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Breadcrumbs?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void ChatControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatBreadcrumb();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Breadcrumbs?.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void ChatControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = controller.ChatBreadcrumb();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
