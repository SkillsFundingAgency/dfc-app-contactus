using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerErrorTests : BaseHomeController
    {
        [Fact]
        public void HomeControllerErrorTestsReturnsSuccess()
        {
            // Arrange
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }
    }
}
