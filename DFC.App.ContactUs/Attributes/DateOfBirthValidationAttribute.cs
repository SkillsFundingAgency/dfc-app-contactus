using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DFC.App.ContactUs.Attributes
{
    [ExcludeFromCodeCoverage]
    public class DateOfBirthValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int yearsAgo;
        private readonly DateTime minimumDate;

        public DateOfBirthValidationAttribute(int yearsAgo, string minimumDateString)
        {
            this.yearsAgo = yearsAgo;
            minimumDate = DateTime.ParseExact(minimumDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private enum ErrorType
        {
            NullDate,
            AllFieldMissing,
            MissingField,
            InvalidDate,
            TooLow,
            TooHigh,
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

            ErrorType errorType = ErrorType.NullDate;
            string missingFieldName = string.Empty;
            string validationFieldName = validationContext.MemberName;

            ErrorMessage = ErrorMessageString;

            if (value is DateViewModel dateViewModel)
            {
                if (!dateViewModel.AllFieldsAreNull)
                {
                    if (!dateViewModel.ContainsNullValueFields)
                    {
                        if (dateViewModel.IsValid)
                        {
                            var dt = dateViewModel.Value;

                            if (dt!.Value.Date >= minimumDate)
                            {
                                if (dt!.Value.Date <= DateTime.Today.AddYears(0 - yearsAgo))
                                {
                                    return ValidationResult.Success;
                                }
                                else
                                {
                                    errorType = ErrorType.TooHigh;
                                    validationFieldName = nameof(dateViewModel.Year);
                                }
                            }
                            else
                            {
                                errorType = ErrorType.TooLow;
                                validationFieldName = nameof(dateViewModel.Year);
                            }
                        }
                        else
                        {
                            errorType = ErrorType.InvalidDate;
                            validationFieldName = nameof(dateViewModel.Day);
                        }
                    }
                    else
                    {
                        errorType = ErrorType.MissingField;
                        missingFieldName = dateViewModel.FirstMissingFieldName;
                        validationFieldName = missingFieldName;
                    }
                }
                else
                {
                    errorType = ErrorType.AllFieldMissing;
                    missingFieldName = dateViewModel.FirstMissingFieldName;
                }
            }

            string errorMessage = errorType switch
            {
                ErrorType.InvalidDate => string.Format(CultureInfo.InvariantCulture, "{0} is not a valid date", validationContext.DisplayName),
                ErrorType.AllFieldMissing => string.Format(CultureInfo.InvariantCulture, "Enter your {0}", validationContext.DisplayName.ToLowerInvariant()),
                ErrorType.MissingField => string.Format(CultureInfo.InvariantCulture, "{0} must include a {1}", validationContext.DisplayName, missingFieldName.ToLowerInvariant()),
                ErrorType.TooLow => string.Format(CultureInfo.InvariantCulture, "You must have been born after {0}", minimumDate.ToString("dd/MM/yyyy")),
                ErrorType.TooHigh => string.Format(CultureInfo.InvariantCulture, "You must be over {0} to use this service", yearsAgo),
                _ => string.Format(CultureInfo.InvariantCulture, ErrorMessage, validationContext.DisplayName),
            };

            return new ValidationResult(errorMessage, new[] { validationFieldName });
        }
    }
}
