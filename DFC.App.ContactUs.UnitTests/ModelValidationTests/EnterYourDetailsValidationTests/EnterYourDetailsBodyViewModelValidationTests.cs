using DFC.App.ContactUs.ViewModels;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidModel()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidModel();

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
