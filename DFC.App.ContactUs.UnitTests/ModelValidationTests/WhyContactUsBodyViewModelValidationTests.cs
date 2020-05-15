using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests
{
    [Trait("Category", "WhyContactUsBodyViewModel Validation Unit Tests")]
    public class WhyContactUsBodyViewModelValidationTests
    {
        [Fact]
        public void WhyContactUsBodyViewModelValidationReturnsSuccessForValidModel()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
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
        public void WhyContactUsBodyViewModelValidationReturnsErrorForCategoryInvalid()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.None,
                MoreDetail = "some more details",
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.SelectedCategory))));
            Assert.Equal(WhyContactUsBodyViewModel.SelectedCategoryValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.SelectedCategory))).ErrorMessage);
        }

        [Fact]
        public void WhyContactUsBodyViewModelValidationReturnsErrorForMoreDetailMissing()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.Website,
                MoreDetail = null,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.MoreDetail))));
            Assert.Equal(WhyContactUsBodyViewModel.MoreDetailRequiredError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.MoreDetail))).ErrorMessage);
        }

        [Fact]
        public void WhyContactUsBodyViewModelValidationReturnsErrorForMoreDetailTooLong()
        {
            // Arrange
            var viewModel = new WhyContactUsBodyViewModel
            {
                SelectedCategory = Category.Website,
                MoreDetail = string.Empty.PadLeft(1001, '-'),
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.MoreDetail))));
            Assert.Equal(WhyContactUsBodyViewModel.MoreDetailInvalidValidationError, validationResults.First(f => f.MemberNames.Any(a => a == nameof(WhyContactUsBodyViewModel.MoreDetail))).ErrorMessage);
        }
    }
}
