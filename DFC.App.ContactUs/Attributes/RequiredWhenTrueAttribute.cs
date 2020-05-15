using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Attributes
{
    public class RequiredWhenTrueAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string comparisonProperty;

        public RequiredWhenTrueAttribute(string? isRequiredProperty)
        {
            _ = isRequiredProperty ?? throw new ArgumentNullException(nameof(isRequiredProperty));

            comparisonProperty = isRequiredProperty;
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

            var property = validationContext.ObjectType.GetProperty(comparisonProperty);

            if (property != null)
            {
                var isRequiredValue = (bool?)property.GetValue(validationContext.ObjectInstance);

                if (isRequiredValue != null && isRequiredValue.Value && value == null)
                {
                    return new ValidationResult(ErrorMessage.Replace("{0}", validationContext.DisplayName, StringComparison.Ordinal), new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
