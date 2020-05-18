﻿using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests.EnterYourDetailsValidationTests
{
    [Trait("Category", "EnterYourDetailsBodyViewModel Validation Unit Tests")]
    public class EnterYourDetailsBodyViewModelCallbackDateTimeValidationTests
    {
        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsSuccessCallbackNotRequired()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = null;
            viewModel.SelectedCategory = Category.AdviceGuidance;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.True(isValid);
            Assert.True(validationResults.Count == 0);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateTimeMissing()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = null;
            viewModel.SelectedCategory = Category.Callback;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))));
            Assert.Contains("Enter your", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))).ErrorMessage, StringComparison.Ordinal);
        }

        [Fact]
        public void EnterYourDetailsBodyViewModelValidationReturnsErrorForCallbackDateTimeNotInTheFuture()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            viewModel.CallbackDateTime = new CallbackDateTimeViewModel(DateTime.Now);
            viewModel.SelectedCategory = Category.Callback;

            // Act
            var (isValid, validationResults) = ModelValidator.TryValidateModel(viewModel);

            // Assert
            Assert.False(isValid);
            Assert.True(validationResults.Count > 0);
            Assert.NotNull(validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))));
            Assert.Contains("must be in the future", validationResults.First(f => f.MemberNames.Any(a => a == nameof(EnterYourDetailsBodyViewModel.CallbackDateTime))).ErrorMessage, StringComparison.Ordinal);
        }
    }
}