using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelTelephoneNumberValidationTests
    {
        public static IEnumerable<object[]> ValidTelephoneNumbers => new List<object[]>
        {
            new string[] { "0123456789" },
            new string[] { "+44 208 1234" },
            new string[] { "+44(208)1234" },
            new string[] { "012081234" },
            new string[] { "012081234 " },
            new string[] { "(0)1234567" },
            new string[] { "+44-208-1234" },
        };

        public static IEnumerable<object[]> InvalidTelephoneNumbers => new List<object[]>
        {
            new string[] { "+44 208 1234 ext 123" },
            new string[] { "+44[208]1234" },
            new string[] { "01208/1234" },
            new string[] { "{0}1234567" },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForTelephoneNumberMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.TelephoneNumber = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForTelephoneNumberTooLong()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.TelephoneNumber = string.Empty.PadLeft(101, '0');

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))));
            Assert.Contains("is limited to between 1 and", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(InvalidTelephoneNumbers))]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForTelephoneNumberInvalid(string telephoneNumber)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.TelephoneNumber = telephoneNumber;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))));
            Assert.Contains("requires numbers only", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.TelephoneNumber))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(ValidTelephoneNumbers))]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidTelephoneNumber(string telephoneNumber)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.TelephoneNumber = telephoneNumber;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
