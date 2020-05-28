using DFC.App.ContactUs.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelDateOfBirthValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForDateOfBirthMissingDateViewModel()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.DateOfBirth = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth))));
            Assert.Contains("Date of birth", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForDateOfBirthMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.DateOfBirth = new DateOfBirthViewModel();

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth))));
            Assert.Contains("Enter your date of birth", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForDateOfBirthNotOver13YearsAgo()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.DateOfBirth = new DateOfBirthViewModel(DateTime.Today.AddYears(-13).AddDays(1));

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth.Year))));
            Assert.Contains("You must be over 13 to use this service", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.DateOfBirth.Year))).ErrorMessage, StringComparison.Ordinal);
        }
    }
}
