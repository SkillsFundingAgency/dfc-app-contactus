﻿using DFC.App.ContactUs.Enums;
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
            new object[] { Category.SomethingElse },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelCallbackMandatoryTest()
        {
            // Arrange
            var expectedErrors = new Dictionary<string, string>
            {
                { nameof(EnterYourDetailsBodyViewModel.FirstName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.FamilyName), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.DateOfBirth), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.Postcode), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.TelephoneNumber), "Enter your" },
                { nameof(EnterYourDetailsBodyViewModel.CallbackDateTime), "Enter when you want us" },
            };
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = Category.Callback,
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
                { nameof(EnterYourDetailsBodyViewModel.FamilyName), "Enter your" },
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
