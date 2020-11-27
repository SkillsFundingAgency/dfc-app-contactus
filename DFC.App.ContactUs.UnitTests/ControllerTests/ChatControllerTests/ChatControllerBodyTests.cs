using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    [Trait("Category", "Chat Controller Unit Tests")]
    public class ChatControllerBodyTests : BaseChatControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ChatControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = await controller.ChatBody().ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<ChatViewBodyModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ChatControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = await controller.ChatBody().ConfigureAwait(false);

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<ChatViewBodyModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task ChatControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = await controller.ChatBody().ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
