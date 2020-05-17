using System;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelTermsAndConditionsAcceptedValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForTermsAndConditionsAcceptedInvalid()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidModel();
            viewModel.TermsAndConditionsAccepted = false;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.ErrorMessage.Contains("Please tick the", StringComparison.Ordinal)));
        }
    }
}
