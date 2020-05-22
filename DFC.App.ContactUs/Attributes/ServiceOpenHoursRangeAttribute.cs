using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.App.ContactUs.Attributes
{
    public class ServiceOpenHoursRangeAttribute : ValidationAttribute
    {
        private readonly string relatedProperty;

        public ServiceOpenHoursRangeAttribute(string? relatedProperty)
        {
            _ = relatedProperty ?? throw new ArgumentNullException(nameof(relatedProperty));

            this.relatedProperty = relatedProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));

            ErrorMessage = ErrorMessageString;

            var property = validationContext.ObjectType.GetProperty(relatedProperty);

            if (property != null)
            {
                var serviceOpenDetailModel = validationContext.GetService(typeof(ServiceOpenDetailModel)) as ServiceOpenDetailModel;
                var otherValue = (int?)property.GetValue(validationContext.ObjectInstance);
                var thisValue = (int?)value;

                if (serviceOpenDetailModel != null && otherValue != null && thisValue != null)
                {
                    TimeSpan? enteredTime = null;

                    if (validationContext.MemberName.Equals(nameof(CallbackDateTimeViewModel.Hour), StringComparison.OrdinalIgnoreCase))
                    {
                        enteredTime = new TimeSpan(thisValue.Value, otherValue.Value, 0);
                    }
                    else if (validationContext.MemberName.Equals(nameof(CallbackDateTimeViewModel.Minute), StringComparison.OrdinalIgnoreCase))
                    {
                        enteredTime = new TimeSpan(otherValue.Value, thisValue.Value, 0);
                    }

                    if (enteredTime != null && (serviceOpenDetailModel.OpenTimeFrom > enteredTime || serviceOpenDetailModel.OpenTimeTo < enteredTime))
                    {
                        var errorMessage = string.Format(CultureInfo.InvariantCulture, ErrorMessage, serviceOpenDetailModel.OpenTimeFrom.ToString(), serviceOpenDetailModel.OpenTimeTo.ToString());

                        return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
