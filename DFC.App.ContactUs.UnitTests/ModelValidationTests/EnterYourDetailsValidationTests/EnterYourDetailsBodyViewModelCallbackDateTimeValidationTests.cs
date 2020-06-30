using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.ViewModels;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelCallbackDateTimeValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessCallbackValid()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateOptionSelected = CallbackDateOption.TodayPlus1;
            viewModel.CallbackTimeOptionSelected = CallbackTimeOption.Band3;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessCallbackNotRequired()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateOptionSelected = null;
            viewModel.CallbackTimeOptionSelected = null;
            viewModel.SelectedCategory = Category.AdviceGuidance;
            viewModel.IsCallback = false;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateOptionSelected = null;
            viewModel.CallbackTimeOptionSelected = CallbackTimeOption.Band3;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateOptionSelected))));
            Assert.Equal(EnterYourDetailsBodyViewModel.CallbackDateOptionValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateOptionSelected))).ErrorMessage);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackTimeMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateOptionSelected = CallbackDateOption.TodayPlus1;
            viewModel.CallbackTimeOptionSelected = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackTimeOptionSelected))));
            Assert.Equal(EnterYourDetailsBodyViewModel.CallbackTimeOptionValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackTimeOptionSelected))).ErrorMessage);
        }
    }
}
