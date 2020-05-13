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
    public class WhyContactUsControllerBodyPostTests : BaseWhyContactUsController
    {
        public static IEnumerable<object[]> ValidSelectedCategories => new List<object[]>
        {
            new object[] { WhyContactUsBodyViewModel.SelectCategory.AdviceGuidance, "/contact-us", },
            new object[] { WhyContactUsBodyViewModel.SelectCategory.Courses, "/contact-us", },
            new object[] { WhyContactUsBodyViewModel.SelectCategory.Website, "/contact-us", },
            new object[] { WhyContactUsBodyViewModel.SelectCategory.Feedback, "/contact-us", },
            new object[] { WhyContactUsBodyViewModel.SelectCategory.SomethingElse, "/contact-us", },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void WhyContactUsControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string expectedRedirectUrl = "/contact-us";
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = WhyContactUsBodyViewModel.SelectCategory.Website,
                MoreDetail = "some more detail",
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
        public void WhyContactUsControllerBodyPostReturnsSuccessForValidCategories(WhyContactUsBodyViewModel.SelectCategory selectedCategory, string expectedRedirectUrl)
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
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public void WhyContactUsControllerBodyPostReturnsErrorForInvalidCategory()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = WhyContactUsBodyViewModel.SelectCategory.None,
                MoreDetail = "some more detail",
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsBodyViewModel>(viewResult.ViewData.Model);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.Contains(nameof(WhyContactUsBodyViewModel.SelectedCategory), viewResult.ViewData.ModelState.Keys);

            controller.Dispose();
        }

        [Fact]
        public void WhyContactUsControllerBodyPostReturnsErrorForInvalidMoreDetail()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = WhyContactUsBodyViewModel.SelectCategory.Website,
                MoreDetail = null,
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.MoreDetail), "Fake error");

            // Act
            var result = controller.WhyContactUsBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsBodyViewModel>(viewResult.ViewData.Model);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.Contains(nameof(WhyContactUsBodyViewModel.MoreDetail), viewResult.ViewData.ModelState.Keys);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void WhyContactUsControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = WhyContactUsBodyViewModel.SelectCategory.None,
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
