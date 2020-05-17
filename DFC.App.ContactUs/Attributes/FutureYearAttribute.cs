using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.App.ContactUs.Attributes
{
    public class FutureYearAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int yearsAhead;

        public FutureYearAttribute(int yearsAhead)
        {
            this.yearsAhead = yearsAhead;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));

            ErrorMessage = ErrorMessageString;

            if (value != null)
            {
                var yearValue = (int)value;
                var fromYear = DateTime.Today.Year;
                var toYear = fromYear + yearsAhead;

                if (yearValue < fromYear || yearValue > toYear)
                {
                    var errorMessage = string.Format(CultureInfo.InvariantCulture, ErrorMessage, validationContext.DisplayName, fromYear, toYear);

                    return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
