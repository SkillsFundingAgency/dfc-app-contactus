using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerViewPostTests : BaseEnterYourDetailsController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void EnterYourDetailsControllerViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            string expectedRedirectUrl = $"/{PagesController.LocalPath}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsView(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public void EnterYourDetailsControllerViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.EnterYourDetailsView(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void EnterYourDetailsControllerViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = Enums.Category.None,
            };
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.EnterYourDetailsView(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
