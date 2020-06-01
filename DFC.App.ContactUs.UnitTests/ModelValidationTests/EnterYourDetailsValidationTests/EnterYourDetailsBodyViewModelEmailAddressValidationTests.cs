using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelEmailAddressValidationTests
    {
        public static IEnumerable<object[]> ValidEmailAddresses => new List<object[]>
        {
            new string[] { "abc@def.com" },
            new string[] { "abc@def.co.uk" },
            new string[] { "a.b-c@abc-123.com" },
        };

        public static IEnumerable<object[]> InvalidEmailAddresses => new List<object[]>
        {
            new string[] { "abc@def." },
            new string[] { "@def.co.uk" },
            new string[] { "abcdev" },
            new string[] { "abcdev.com" },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForEmailAddressMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.SelectedCategory = Enums.Category.SomethingElse;
            viewModel.EmailAddress = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForEmailAddressTooLong()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.EmailAddress = string.Empty.PadLeft(101, '0');

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))));
            Assert.Contains("is limited to between 1 and", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(InvalidEmailAddresses))]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForEmailAddressInvalid(string emailAddress)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.EmailAddress = emailAddress;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))));
            Assert.Contains("Enter a valid email address", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.EmailAddress))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(ValidEmailAddresses))]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidEmailAddress(string emailAddress)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.EmailAddress = emailAddress;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
