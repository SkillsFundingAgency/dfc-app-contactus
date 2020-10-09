using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DFC.App.ContactUs.Attributes
{
    [ExcludeFromCodeCoverage]
    public class CallbackTimeOptionValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string callbackDateOptionProperty;

        public CallbackTimeOptionValidatorAttribute(string? callbackDateOptionProperty)
        {
            _ = callbackDateOptionProperty ?? throw new ArgumentNullException(nameof(callbackDateOptionProperty));

            this.callbackDateOptionProperty = callbackDateOptionProperty;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var error = string.Format(CultureInfo.InvariantCulture, ErrorMessage, context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));

            ErrorMessage = ErrorMessageString;

            var property = validationContext.ObjectType.GetProperty(callbackDateOptionProperty);
            if (property != null && value != null)
            {
                var callbackTimeOptionValue = (CallbackTimeOption)value;
                var callbackDateOption = (CallbackDateOption?)property.GetValue(validationContext.ObjectInstance);

                if (callbackDateOption == CallbackDateOption.Today && EnterYourDetailsBodyViewModel.FirstDateIsForToday && EnterYourDetailsBodyViewModel.DisabledTimeBands[callbackTimeOptionValue])
                {
                    var errorMessage = string.Format(CultureInfo.InvariantCulture, ErrorMessage, validationContext.DisplayName.ToLowerInvariant());
                    return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
