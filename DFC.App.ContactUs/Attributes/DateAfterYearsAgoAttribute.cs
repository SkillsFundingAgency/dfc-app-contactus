using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Attributes
{
    public class DateAfterYearsAgoAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int yearsAgo;

        public DateAfterYearsAgoAttribute(int yearsAgo)
        {
            this.yearsAgo = yearsAgo;
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
            DateTime? dt = null;
            bool isValidDate = false;

            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));

            ErrorMessage = ErrorMessageString;

            if (value is DateViewModel dateViewModel)
            {
                isValidDate = dateViewModel.IsValid;
                if (isValidDate)
                {
                    dt = dateViewModel.Value;
                }
            }
            else if (value != null)
            {
                dt = (DateTime)value;
                isValidDate = true;
            }

            if (isValidDate)
            {
                if (dt!.Value.Date <= DateTime.Today.AddYears(yearsAgo))
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult($"{validationContext.DisplayName} is not a valid date", new[] { validationContext.MemberName });
            }

            return new ValidationResult(ErrorMessage.Replace("{0}", validationContext.DisplayName, StringComparison.Ordinal), new[] { validationContext.MemberName });
        }
    }
}
