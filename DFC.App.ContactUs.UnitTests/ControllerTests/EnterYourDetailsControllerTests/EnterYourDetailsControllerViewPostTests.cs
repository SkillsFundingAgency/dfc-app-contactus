using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.AreaRoutingService.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerViewPostTests : BaseEnterYourDetailsControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task EnterYourDetailsControllerViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string expectedEmailTemplate = "An email template";
            const bool expectedSendEmailResult = true;
            string expectedRedirectUrl = $"/{PagesController.LocalPath}/{HomeController.ThankyouForContactingUsCanonicalName}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            viewModel.SelectedCategory = Enums.Category.Callback;

            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).Returns(expectedEmailTemplate);
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).Returns(A.Dummy<RoutingDetailModel>());
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).Returns(A.Fake<ContactUsEmailRequestModel>());
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).Returns(expectedSendEmailResult);

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public async Task EnterYourDetailsControllerViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task EnterYourDetailsControllerViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustNotHaveHappened();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }

        [Fact]
        public async Task EnterYourDetailsControllerViewPostReturnsSameViewForMissingEmailTemplate()
        {
            // Arrange
            string expectedEmailTemplate = string.Empty;
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).Returns(expectedEmailTemplate);

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task EnterYourDetailsControllerViewPostReturnsSuccessForMissingRouting()
        {
            // Arrange
            const bool expectedSendEmailResult = true;
            const string expectedEmailTemplate = "An email template";
            RoutingDetailModel? expectedRoutingDetailModel = null;
            string expectedRedirectUrl = $"/{PagesController.LocalPath}/{HomeController.ThankyouForContactingUsCanonicalName}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).Returns(expectedEmailTemplate);
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).Returns(expectedRoutingDetailModel);
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).Returns(A.Fake<ContactUsEmailRequestModel>());
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).Returns(expectedSendEmailResult);

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeRoutingService.GetAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }
    }
}
