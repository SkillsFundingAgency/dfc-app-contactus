using DFC.App.ContactUs.Enums;
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
        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new object[] { Category.AdviceGuidance },
            new object[] { Category.Courses },
            new object[] { Category.Feedback },
            new object[] { Category.SomethingElse },
        };

        [Fact]
        public void EnterYourDetailsBodyViewModelCallbackMandatoryTest()
        {
            // Arrange
            var expectedErrorNames = new List<string>
            {
                nameof(EnterYourDetailsBodyViewModel.FirstName),
                nameof(EnterYourDetailsBodyViewModel.FamilyName),
                nameof(EnterYourDetailsBodyViewModel.DateOfBirth),
                nameof(EnterYourDetailsBodyViewModel.Postcode),
                nameof(EnterYourDetailsBodyViewModel.TelephoneNumber),
                nameof(EnterYourDetailsBodyViewModel.CallbackDateTime),
            };
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = Category.Callback,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count == expectedErrorNames.Count + 1);

            expectedErrorNames.ForEach(fe =>
                {
                    Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == fe)));
                    Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == fe)).ErrorMessage, StringComparison.Ordinal);
                });

            Assert.NotNull(validationResults.First(f => f.ErrorMessage.Contains("Please tick the", StringComparison.Ordinal)));
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void EnterYourDetailsBodyViewModelnonCallbackMandatoryTest(Category category)
        {
            // Arrange
            var expectedErrorNames = new List<string>
            {
                nameof(EnterYourDetailsBodyViewModel.FirstName),
                nameof(EnterYourDetailsBodyViewModel.FamilyName),
                nameof(EnterYourDetailsBodyViewModel.DateOfBirth),
                nameof(EnterYourDetailsBodyViewModel.Postcode),
                nameof(EnterYourDetailsBodyViewModel.EmailAddress),
            };
            var viewModel = new EnterYourDetailsBodyViewModel
            {
                SelectedCategory = category,
            };

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count == expectedErrorNames.Count + 1);

            expectedErrorNames.ForEach(fe =>
            {
                Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == fe)));
                Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == fe)).ErrorMessage, StringComparison.Ordinal);
            });

            Assert.NotNull(validationResults.First(f => f.ErrorMessage.Contains("Please tick the", StringComparison.Ordinal)));
        }
    }
}
