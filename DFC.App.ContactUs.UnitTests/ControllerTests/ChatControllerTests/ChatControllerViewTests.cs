using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    [Trait("Category", "Chat Controller Unit Tests")]
    public class ChatControllerViewTests : BaseChatControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ChatControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            A.CallTo(() => FakeMapper.Map<ChatViewBodyModel>(A<ChatOptions>.Ignored)).Returns(A.Fake<ChatViewBodyModel>());

            // Act
            var result = await controller.ChatView().ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<ChatViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ChatControllerViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            A.CallTo(() => FakeMapper.Map<ChatViewBodyModel>(A<ChatOptions>.Ignored)).Returns(A.Fake<ChatViewBodyModel>());

            // Act
            var result = await controller.ChatView().ConfigureAwait(false);

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<ChatViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task ChatControllerChatViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildChatController(mediaTypeName);

            // Act
            var result = await controller.ChatView().ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
