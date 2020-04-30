using DFC.App.ContactUs.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DFC.App.ContactUs.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LowerCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var result = false;

            switch (value)
            {
                case IEnumerable<string> list:
                    result = list.All(s => s.Equals(s, StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    string item = value.ToString() ?? string.Empty;
                    result = item.Equals(item, StringComparison.OrdinalIgnoreCase);
                    break;
            }

            return result ? ValidationResult.Success
                : new ValidationResult(string.Format(CultureInfo.InvariantCulture, ValidationMessage.FieldNotLowercase, validationContext.DisplayName), new[] { validationContext.MemberName });
        }
    }
}
