using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelCallbackDateTimeValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessCallbackNotRequired()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = null;
            viewModel.SelectedCategory = Category.AdviceGuidance;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateTimeMissingDateViewModel()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = null;
            viewModel.SelectedCategory = Category.Callback;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))));
            Assert.Contains("Enter the callback date and time", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateTimeMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = new CallbackDateTimeViewModel();
            viewModel.SelectedCategory = Category.Callback;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))));
            Assert.Contains("Enter the callback date and time", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateTimeNotInTheFuture()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = new CallbackDateTimeViewModel(DateTime.Now.AddMinutes(-1));
            viewModel.SelectedCategory = Category.Callback;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime.Month))));
            Assert.Contains("must be within", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime.Month))).ErrorMessage, StringComparison.Ordinal);
        }
    }
}
