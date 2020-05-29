using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WhyContactUsControllerTests
{
    [Trait("Category", "WhyContactUs Controller Unit Tests")]
    public class WhyContactUsControllerViewPostTests : BaseWhyContactUsControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedCategories => new List<object[]>
        {
            new object[] { Category.AdviceGuidance, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.AdviceGuidance}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}=" + WebUtility.UrlEncode($"Some {Category.AdviceGuidance} details") },
            new object[] { Category.Courses, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.Courses}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}=" + WebUtility.UrlEncode($"Some {Category.Courses} details") },
            new object[] { Category.Website, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.Website}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}=" + WebUtility.UrlEncode($"Some {Category.Website} details") },
            new object[] { Category.Feedback, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.Feedback}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}=" + WebUtility.UrlEncode($"Some {Category.Feedback} details") },
            new object[] { Category.SomethingElse, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.SomethingElse}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}=" + WebUtility.UrlEncode($"Some {Category.SomethingElse} details") },
        };

        public static IEnumerable<object[]> InvalidSelectedCategories => new List<object[]>
        {
            new object[] { Category.None },
            new object[] { Category.Callback },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void WhyContactUsControllerViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const Category selectedCategory = Category.AdviceGuidance;
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={selectedCategory}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}={WebUtility.UrlEncode(moreDetail)}";
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsView(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(ValidSelectedCategories))]
        public void WhyContactUsControllerViewPostRedirectsSuccessfully(Category selectedCategory, string expectedRedirectUrl)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = $"Some {selectedCategory} details",
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.WhyContactUsView(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidSelectedCategories))]
        public void WhyContactUsControllerViewPostReturnsSameViewForInvalidCategory(Category selectedCategory)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = "Some more details",
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.WhyContactUsView(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public void WhyContactUsControllerViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel();
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.WhyContactUsView(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void WhyContactUsControllerViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.None,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.WhyContactUsView(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
