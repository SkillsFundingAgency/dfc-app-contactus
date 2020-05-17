using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests
{
    [Trait("Category", "HomeBodyViewModel Validation Unit Tests")]
    public class HomeBodyViewModelValidationTests
    {
        [Fact]
        public void HomeBodyViewModelValidationReturnsSuccessForValidModel()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Callback,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void HomeBodyViewModelValidationReturnsErrorForOptionInvalid()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.None,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(HomeBodyViewModel.SelectedOption))));
            Assert.Equal(HomeBodyViewModel.SelectedOptionValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(HomeBodyViewModel.SelectedOption))).ErrorMessage);
        }
    }
}
