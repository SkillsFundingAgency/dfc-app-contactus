using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelFirstNameValidationTests
    {
        public static IEnumerable<object[]> ValidFirstNames => new List<object[]>
        {
            new string[] { "Mark Anthony" },
            new string[] { "Mark-Anthony" },
            new string[] { "Mark Anthony II" },
            new string[] { "M. Anthony" },
            new string[] { "M. Anthony, Tony" },
        };

        public static IEnumerable<object[]> InvalidFirstNames => new List<object[]>
        {
            new string[] { "Mark (Anthony)" },
            new string[] { "Mark [Anthony]" },
            new string[] { "Mark Anthony 2nd" },
            new string[] { "1234" },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFirstNameMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FirstName = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFirstNameTooLong()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FirstName = string.Empty.PadLeft(101, 'a');

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))));
            Assert.Contains("is too long or contains invalid characters", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(InvalidFirstNames))]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFirstNameInvalid(string firstname)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FirstName = firstname;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))));
            Assert.Contains("is too long or contains invalid characters", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FirstName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(ValidFirstNames))]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidFirstName(string firstname)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FirstName = firstname;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
