using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.ViewModels;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests
{
    [Trait("Category", "HowCanWeHelpBodyViewModel Validation Unit Tests")]
    public class HowCanWeHelpBodyViewModelValidationTests
    {
        [Fact]
        public void HowCanWeHelpBodyViewModelValidationReturnsSuccessForValidModel()
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = Category.Website,
                MoreDetail = "some more details",
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void HowCanWeHelpBodyViewModelValidationReturnsErrorForCategoryInvalid()
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = Category.None,
                MoreDetail = "some more details",
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.SelectedCategory))));
            Assert.Equal(HowCanWeHelpBodyViewModel.SelectedCategoryValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.SelectedCategory))).ErrorMessage);
        }

        [Fact]
        public void HowCanWeHelpBodyViewModelValidationReturnsErrorForMoreDetailMissing()
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = Category.Website,
                MoreDetail = null,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.MoreDetail))));
            Assert.Equal(HowCanWeHelpBodyViewModel.MoreDetailRequiredError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.MoreDetail))).ErrorMessage);
        }

        [Fact]
        public void HowCanWeHelpBodyViewModelValidationReturnsErrorForMoreDetailTooLong()
        {
            // Arrange
            var viewModel = new HowCanWeHelpBodyViewModel
            {
                SelectedCategory = Category.Website,
                MoreDetail = string.Empty.PadLeft(1001, '-'),
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.MoreDetail))));
            Assert.Equal("Message is limited to between 1 and 1000 characters", validationResults.First(f => f.MemberNames.Any(a => a == nameof(HowCanWeHelpBodyViewModel.MoreDetail))).ErrorMessage);
        }
    }
}
