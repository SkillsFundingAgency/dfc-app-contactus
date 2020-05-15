using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerBodyPostTests : BaseEnterYourDetailsController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void EnterYourDetailsControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string expectedRedirectUrl = "/contact-us";
            var viewModel = ValidModelBuilders.BuildValidModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            // Act
            var result = controller.EnterYourDetailsBody(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public void EnterYourDetailsControllerBodyPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.TermsAndConditionsAccepted), "Fake error");

            // Act
            var result = controller.EnterYourDetailsBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void EnterYourDetailsControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                TermsAndConditionsAccepted = false,
            };
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.FirstName), "Fake error");

            // Act
            var result = controller.EnterYourDetailsBody(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
