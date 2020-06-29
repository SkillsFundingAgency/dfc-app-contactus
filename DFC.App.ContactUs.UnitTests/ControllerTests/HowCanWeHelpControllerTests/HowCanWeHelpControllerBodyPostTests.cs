using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HowCanWeHelpControllerTests
{
    [Trait("Category", "HowCanWeHelp Controller Unit Tests")]
    public class HowCanWeHelpControllerBodyPostTests : BaseHowCanWeHelpControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedCategories => new List<object[]>
        {
            new object[] { Category.AdviceGuidance, },
            new object[] { Category.Courses, },
            new object[] { Category.Website, },
            new object[] { Category.Feedback, },
            new object[] { Category.Other, },
        };

        public static IEnumerable<object[]> InvalidSelectedCategories => new List<object[]>
        {
            new object[] { Category.None },
            new object[] { Category.Callback },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HowCanWeHelpControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const Category selectedCategory = Category.Website;
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}";
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.HowCanWeHelpBody(viewModel).ConfigureAwait(false);

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
        public async Task HowCanWeHelpControllerBodyPostReturnsSuccessForValidCategories(Category selectedCategory)
        {
            // Arrange
            string moreDetail = $"Some {selectedCategory} details";
            string expectedRedirectUrl = $"/{RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}";
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = moreDetail,
            };
            var controller = BuildHowCanWeHelpController(MediaTypeNames.Text.Html);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.HowCanWeHelpBody(viewModel).ConfigureAwait(false);

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
        public async Task HowCanWeHelpControllerBodyPostReturnsSameViewForInvalidCategory(Category selectedCategory)
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = selectedCategory,
                MoreDetail = "some more detail",
            };
            var controller = BuildHowCanWeHelpController(MediaTypeNames.Text.Html);

            // Act
            var result = await controller.HowCanWeHelpBody(viewModel).ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HowCanWeHelpBodyViewModel>(viewResult.ViewData.Model);

            model.SelectedCategory.Should().Be(selectedCategory);

            controller.Dispose();
        }

        [Fact]
        public async Task HowCanWeHelpControllerBodyPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel();
            var controller = BuildHowCanWeHelpController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(HowCanWeHelpBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.HowCanWeHelpBody(viewModel).ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HowCanWeHelpBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HowCanWeHelpControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = Category.None,
            };
            var controller = BuildHowCanWeHelpController(mediaTypeName);

            // Act
            var result = await controller.HowCanWeHelpBody(viewModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
