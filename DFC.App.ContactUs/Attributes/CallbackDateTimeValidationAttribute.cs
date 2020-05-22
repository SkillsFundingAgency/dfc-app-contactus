using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.App.ContactUs.Attributes
{
    public class CallbackDateTimeValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int monthsInFuture;
        private readonly string comparisonProperty;

        public CallbackDateTimeValidationAttribute(int monthsInFuture, string? comparisonProperty)
        {
            _ = comparisonProperty ?? throw new ArgumentNullException(nameof(comparisonProperty));

            this.monthsInFuture = monthsInFuture;
            this.comparisonProperty = comparisonProperty;
        }

        private enum ErrorType
        {
            NullDate,
            InvalidDate,
            OutOfRange,
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
            ErrorType errorType = ErrorType.NullDate;

            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));

            ErrorMessage = ErrorMessageString;

            if (!IsRequired(validationContext))
            {
                return ValidationResult.Success;
            }

            if (value is DateViewModel dateViewModel && !dateViewModel.IsNull)
            {
                if (dateViewModel.IsValid)
                {
                    var dt = dateViewModel.Value;

                    if (dt!.Value > DateTime.Now && dt.Value <= DateTime.Now.AddMonths(monthsInFuture))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        errorType = ErrorType.OutOfRange;
                    }
                }
                else
                {
                    errorType = ErrorType.InvalidDate;
                }
            }

            string errorMessage = errorType switch
            {
                ErrorType.InvalidDate => string.Format(CultureInfo.InvariantCulture, "{0} is not a valid date", validationContext.DisplayName),
                ErrorType.OutOfRange => string.Format(CultureInfo.InvariantCulture, "{0} must be within {1} months", validationContext.DisplayName, monthsInFuture),
                _ => string.Format(CultureInfo.InvariantCulture, ErrorMessage, validationContext.DisplayName),
            };

            return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
        }

        private bool IsRequired(ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(comparisonProperty);

            if (property != null)
            {
                var isRequiredValue = (bool?)property.GetValue(validationContext.ObjectInstance);

                return isRequiredValue != null && isRequiredValue.Value;
            }

            return false;
        }
    }
}
