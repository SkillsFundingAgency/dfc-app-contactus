using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WhyContactUsControllerTests
{
    [Trait("Category", "WhyContactUs Controller Unit Tests")]
    public class WhyContactUsControllerBodyPostTests : BaseWhyContactUsControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedCategories => new List<object[]>
        {
            new object[] { Category.AdviceGuidance, },
            new object[] { Category.Courses, },
            new object[] { Category.Website, },
            new object[] { Category.Feedback, },
            new object[] { Category.SomethingElse, },
        };

        public static IEnumerable<object[]> InvalidSelectedCategories => new List<object[]>
        {
            new object[] { Category.None },
            new object[] { Category.Callback },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void WhyContactUsControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const Category selectedCategory = Category.Website;
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={selectedCategory}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}={WebUtility.UrlEncode(moreDetail)}";
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(ValidSelectedCategories))]
        public void WhyContactUsControllerBodyPostReturnsSuccessForValidCategories(Category selectedCategory)
        {
            // Arrange
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={selectedCategory}&{nameof(WhyContactUsBodyViewModel.MoreDetail)}={WebUtility.UrlEncode(moreDetail)}";
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidSelectedCategories))]
        public void WhyContactUsControllerBodyPostReturnsSameViewForInvalidCategory(Category selectedCategory)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = "some more detail",
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<WhyContactUsBodyViewModel>(viewResult.ViewData.Model);

            model.SelectedCategory.Should().Be(selectedCategory);

            controller.Dispose();
        }

        [Fact]
        public void WhyContactUsControllerBodyPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel();
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void WhyContactUsControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.None,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
