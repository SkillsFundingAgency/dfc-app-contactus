using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WhyContactUsControllerTests
{
    [Trait("Category", "WhyContactUs Controller Unit Tests")]
    public class WhyContactUsControllerViewPostTests : BaseWhyContactUsControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedCategories => new List<object[]>
        {
            new object[] { Category.AdviceGuidance, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
            new object[] { Category.Courses, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
            new object[] { Category.Website, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
            new object[] { Category.Feedback, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
            new object[] { Category.SomethingElse, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
        };

        public static IEnumerable<object[]> InvalidSelectedCategories => new List<object[]>
        {
            new object[] { Category.None },
            new object[] { Category.Callback },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task WhyContactUsControllerViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const Category selectedCategory = Category.AdviceGuidance;
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}";
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.WhyContactUsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(ValidSelectedCategories))]
        public async Task WhyContactUsControllerViewPostRedirectsSuccessfully(Category selectedCategory, string expectedRedirectUrl)
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = $"Some {selectedCategory} details",
            };
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.WhyContactUsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidSelectedCategories))]
        public async Task WhyContactUsControllerViewPostReturnsSameViewForInvalidCategory(Category selectedCategory)
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
            var result = await controller.WhyContactUsView(viewModel).ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task WhyContactUsControllerViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel();
            var controller = BuildWhyContactUsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.WhyContactUsView(viewModel).ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task WhyContactUsControllerViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.None,
            };
            var controller = BuildWhyContactUsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(WhyContactUsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.WhyContactUsView(viewModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
