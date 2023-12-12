using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DFC.App.ContactUs.Attributes
{
    public class CustomUKPostcodeValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string RegExForPostcode = "^([bB][fF][pP][oO]\\s{0,1}[0-9]{1,4}|[gG][iI][rR]\\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";
        private const string RegExForExcludePostcode = "^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$";

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
            string validationFieldName = validationContext.MemberName;
            string errorMessage = "Postcode must be an English or BFPO postcode";
            if (Regex.IsMatch(value.ToString(), RegExForPostcode))
            {
                // Exclude Scotland, Wales, and North Ireland postcodes
                if (Regex.IsMatch(value.ToString(), RegExForExcludePostcode))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(errorMessage, new[] { validationFieldName });
        }
    }
}
