using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DFC.App.ContactUs.Attributes
{
    public class CustomUKPostcodeValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string RegExForPostcode = "^([bB][fF][pP][oO]\\s{0,1}[0-9]{1,4}|[gG][iI][rR]\\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";

        private string[] scotlandWalesNorthirelandPostcodes =
        {
            "ZE", "KW", "IV", "HS", "PH", "AB", "DD", "PA", "FK", "DG",
            "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9", "TD",
            "KY", "KA", "EH", "ML", "LL", "SY", "LD", "SA", "CF", "BT",
        };

        public void AddValidation(ClientModelValidationContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var error = string.Format(CultureInfo.InvariantCulture, ErrorMessage, context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }

        protected override ValidationResult IsValid(object value,
                                                    ValidationContext validationContext)
        {
            _ = validationContext ?? throw new ArgumentNullException(nameof(validationContext));
            string validationFieldName = validationContext.MemberName;
            string errorMessage = "Postcode must be an English or BFPO postcode";

            string postcode = value.ToString();
            if (Regex.IsMatch(postcode, RegExForPostcode))
            {
                if (postcode != null)
                {
                    if (scotlandWalesNorthirelandPostcodes.ToList().Contains(postcode[..2].ToUpperInvariant()))
                    {
                        return new ValidationResult(errorMessage, new[] { validationFieldName });
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            return new ValidationResult(errorMessage, new[] { validationFieldName });
        }
    }
}
