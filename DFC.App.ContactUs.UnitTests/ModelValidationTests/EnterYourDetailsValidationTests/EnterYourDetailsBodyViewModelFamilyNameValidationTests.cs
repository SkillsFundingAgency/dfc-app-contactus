using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelFamilyNameValidationTests
    {
        public static IEnumerable<object[]> ValidFamilyNames => new List<object[]>
        {
            new string[] { "Mark Anthony" },
            new string[] { "Mark-Anthony" },
            new string[] { "Mark Anthony II" },
            new string[] { "M. Anthony" },
            new string[] { "M. Anthony, Tony" },
        };

        public static IEnumerable<object[]> InvalidFamilyNames => new List<object[]>
        {
            new string[] { "Mark (Anthony)" },
            new string[] { "Mark [Anthony]" },
            new string[] { "Mark Anthony 2nd" },
            new string[] { "1234" },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFamilyNameMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FamilyName = null;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFamilyNameTooLong()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FamilyName = string.Empty.PadLeft(101, 'a');

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))));
            Assert.Contains("is limited to between 1 and", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(InvalidFamilyNames))]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForFamilyNameInvalid(string familyName)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FamilyName = familyName;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))));
            Assert.Contains("contains invalid characters", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.FamilyName))).ErrorMessage, StringComparison.Ordinal);
        }

        [Theory]
        [MemberData(nameof(ValidFamilyNames))]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessForValidFamilyName(string familyName)
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.FamilyName = familyName;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }
    }
}
