using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelPostcodeValidationTests
    {
        public static IEnumerable<object[]> ValidPostcodes => new List<object[]>
        {
            new string[] { "CV12AB" },
            new string[] { "CV1 2AB" },
            new string[] { "N1 1aa" },
            new string[] { "w1a 1aa" },
        };

        public static IEnumerable<object[]> InvalidPostcodes => new List<object[]>
        {
            new string[] { "CV1" },
            new string[] { "N1 11a" },
            new string[] { "nw1-2aa" },
            new string[] { "AB10 1AB" }, /*Aberdeen*/
            new string[] { "EH1 1AE" }, /*Edinburgh*/
            new string[] { "BT1 1AR" }, /*Belfast*/
            new string[] { "CF10 1AA" }, /*Cardiff*/
            new string[] { "LL30 1AB" }, /*Llandudno*/
            new string[] { "SA1 1AF" }, /*Swansea*/
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForPostcodeMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.Postcode = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForPostcodeTooLong()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.Postcode = string.Empty.PadLeft(9, '0');

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))));
            Assert.Contains("is limited to between 1 and", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(InvalidPostcodes))]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForPostcodeInvalid(string postcode)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.Postcode = postcode;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))));
            Assert.Contains("Postcode must be an English", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.Postcode))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(ValidPostcodes))]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidPostcode(string postcode)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.Postcode = postcode;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
