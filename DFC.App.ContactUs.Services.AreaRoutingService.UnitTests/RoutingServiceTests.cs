using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.AreaRoutingService.UnitTests
{
    public class RoutingServiceTests
    {
        private const string ValidPostcode = "CV1 1CV";

        private readonly IApiDataProcessorService fakeApiDataProcessorService = A.Fake<IApiDataProcessorService>();
        private readonly HttpClient fakeHttpClient = A.Fake<HttpClient>();

        private FamApiRoutingOptions FamApiRoutingOptions => new FamApiRoutingOptions
        {
            BaseAddress = new Uri("https://localhost/", UriKind.Absolute),
            AreaRoutingEndpoint = "api/something",
            FallbackEmailToAddresses = "fallbackEmail",
            ProblemsEmailAddress = "problemEmail",
            FeebackEmailAddress = "feedbackEmail",
            OtherEmailAddress = "otherEmail",
        };

        [Theory]
        [InlineData(Category.Feedback, "feedbackEmail")]
        [InlineData(Category.Website, "problemEmail")]
        [InlineData(Category.Other, "otherEmail")]
        public async Task ForDirectCategoriesReturnsCorrrectEmail(Category selectedCategory, string expectedEmail)
        {
            // arrange
            var routingService = new RoutingService(FamApiRoutingOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await routingService.GetEmailToSendTo(ValidPostcode, selectedCategory).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
            result.Should().Be(expectedEmail);
        }

        [Theory]
        [InlineData(Category.AdviceGuidance)]
        [InlineData(Category.Courses)]
        public async Task ForAdviceAndCoursesGetsEmailFromFam(Category selectedCategory)
        {
            // arrange
            var routingDetailModel = new RoutingDetailModel()
            {
                EmailAddress = "areaEmail",
            };
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(routingDetailModel);

            var routingService = new RoutingService(FamApiRoutingOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await routingService.GetEmailToSendTo(ValidPostcode, selectedCategory).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            result.Should().Be(routingDetailModel.EmailAddress);
        }

        [Fact]
        public async Task WhenFamFailsToReturnEmailDefaultsToTheFallBack()
        {
            // arrange
            var routingDetailModel = new RoutingDetailModel()
            {
                EmailAddress = null,
            };
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(routingDetailModel);

            var routingService = new RoutingService(FamApiRoutingOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            var result = await routingService.GetEmailToSendTo(ValidPostcode, Category.AdviceGuidance).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeApiDataProcessorService.GetAsync<RoutingDetailModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            result.Should().Be(FamApiRoutingOptions.FallbackEmailToAddresses);
        }

        [Fact]
        public void UnsupportedCategoryCauseException()
        {
            // arrange
            var routingService = new RoutingService(FamApiRoutingOptions, fakeApiDataProcessorService, fakeHttpClient);

            // act
            Func<Task> act = async () => await routingService.GetEmailToSendTo(ValidPostcode, Category.None).ConfigureAwait(false);

            // assert
            act.Should().ThrowExactlyAsync<InvalidEnumArgumentException>();
        }
    }
}
