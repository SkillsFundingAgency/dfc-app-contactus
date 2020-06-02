using DFC.App.ContactUs.Models;
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
            AllFieldMissing,
            MissingField,
            InvalidDate,
            OutOfRange,
            ServiceOpenHoursDay,
            ServiceOpenHoursTime,
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
            DayOfWeek wrongDayOfWeek = DayOfWeek.Monday;
            string validationFieldName = validationContext.MemberName;
            ServiceOpenDetailModel? serviceOpenDetailModel = null;

            ErrorMessage = ErrorMessageString;

            if (!IsRequired(comparisonProperty, validationContext))
            {
                return ValidationResult.Success;
            }

            if (value is DateViewModel dateViewModel)
            {
                if (!dateViewModel.AllFieldsAreNull)
                {
                    if (!dateViewModel.ContainsNullValueFields)
                    {
                        if (dateViewModel.IsValid)
                        {
                            var dt = dateViewModel.Value;

                            if (dt!.Value > DateTime.Now && dt.Value <= DateTime.Now.AddMonths(monthsInFuture))
                            {
                                serviceOpenDetailModel = validationContext.GetService(typeof(ServiceOpenDetailModel)) as ServiceOpenDetailModel ?? new ServiceOpenDetailModel();

                                if (serviceOpenDetailModel != null)
                                {
                                    if (serviceOpenDetailModel.IsServiceOpenForDay(dt!.Value.DayOfWeek))
                                    {
                                        if (serviceOpenDetailModel.OpenTimeFrom <= dt!.Value.TimeOfDay && serviceOpenDetailModel.OpenTimeTo >= dt!.Value.TimeOfDay)
                                        {
                                            return ValidationResult.Success;
                                        }
                                        else
                                        {
                                            errorType = ErrorType.ServiceOpenHoursTime;
                                            validationFieldName = nameof(dateViewModel.Hour);
                                        }
                                    }
                                    else
                                    {
                                        errorType = ErrorType.ServiceOpenHoursDay;
                                        wrongDayOfWeek = dt!.Value.DayOfWeek;
                                        validationFieldName = nameof(dateViewModel.Day);
                                    }
                                }
                            }
                            else
                            {
                                errorType = ErrorType.OutOfRange;
                                validationFieldName = nameof(dateViewModel.Month);
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
                ErrorType.AllFieldMissing => ErrorMessage,
                ErrorType.MissingField => string.Format(CultureInfo.InvariantCulture, "{0} must include a {1}", validationContext.DisplayName, missingFieldName.ToLowerInvariant()),
                ErrorType.OutOfRange => string.Format(CultureInfo.InvariantCulture, "{0} must be within {1} months", validationContext.DisplayName, monthsInFuture),
                ErrorType.ServiceOpenHoursTime => string.Format(CultureInfo.InvariantCulture, "Service opening hours are between {0} and {1}", serviceOpenDetailModel!.OpenTimeFromString, serviceOpenDetailModel!.OpenTimeToString),
                ErrorType.ServiceOpenHoursDay => string.Format(CultureInfo.InvariantCulture, "Service opening hours are between {0} and {1}, {2}, this date is a {3}", serviceOpenDetailModel!.OpenTimeFromString, serviceOpenDetailModel!.OpenTimeToString, serviceOpenDetailModel!.OpenDays, wrongDayOfWeek),
                _ => ErrorMessage,
            };

            return new ValidationResult(errorMessage, new[] { validationFieldName });
        }

        private static bool IsRequired(string propertyName, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(propertyName);

            if (property != null)
            {
                var isRequiredValue = (bool?)property.GetValue(validationContext.ObjectInstance);

                return isRequiredValue != null && isRequiredValue.Value;
            }

            return false;
        }
    }
}
