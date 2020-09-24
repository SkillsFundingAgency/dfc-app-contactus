using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelMandatoryItemsTests
    {
        public static IEnumerable<object[]> Categories => new List<object[]>
        {
            new object[] { Category.AdviceGuidance },
            new object[] { Category.Courses },
            new object[] { Category.Website },
            new object[] { Category.Feedback },
            new object[] { Category.Other },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelCallbackMandatoryTest()
        {
            // Arrange
            var expectedErrors = new Dictionary<string, string>
            {
                { nameof(EnterYourDetailsBodyViewModel.FirstName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.LastName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.DateOfBirth), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.Postcode), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.TelephoneNumber), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.CallbackDateOptionSelected), EnterYourDetailsBodyViewModel.CallbackDateOptionValidationError },
                { nameof(EnterYourDetailsBodyViewModel.CallbackTimeOptionSelected), EnterYourDetailsBodyViewModel.CallbackTimeOptionValidationError },
            };
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                IsCallback = true,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count == expectedErrors.Count + 1);

            expectedErrors.Keys.ToList().ForEach(fe =>
                {
                    Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == fe)));
                    Assert.Contains(expectedErrors[fe], validationResults.First(f => f.MemberNames.Any(a => a == fe)).ErrorMessage, StringComparison.Ordinal);
                });

            Assert.NotNull(validationResults.First(f => f.ErrorMessage.Contains("You must accept our", StringComparison.Ordinal)));
        }

        [Theory]
        [MemberData(nameof(Categories))]
        public void EnterYourDetailsBodyViewModelnonCallbackMandatoryTest(Category category)
        {
            // Arrange
            var expectedErrors = new Dictionary<string, string>
            {
                { nameof(EnterYourDetailsBodyViewModel.FirstName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.LastName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.DateOfBirth), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.Postcode), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.EmailAddress), "Enter your" },
            };
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = category,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count == expectedErrors.Count + 1);

            expectedErrors.Keys.ToList().ForEach(fe =>
            {
                Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == fe)));
                Assert.Contains(expectedErrors[fe], validationResults.First(f => f.MemberNames.Any(a => a == fe)).ErrorMessage, StringComparison.Ordinal);
            });

            Assert.NotNull(validationResults.First(f => f.ErrorMessage.Contains("You must accept our", StringComparison.Ordinal)));
        }
    }
}
