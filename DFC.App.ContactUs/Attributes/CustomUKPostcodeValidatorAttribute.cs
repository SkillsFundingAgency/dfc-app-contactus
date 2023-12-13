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
        private const string RegExForExcludePostcode = "^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$";

        private const string RegExForScotPostcode = "/^(AB3[7-8]|AB5[4-6]|FK(1[7-9]|2[0-1])|(HS|ZE)[1-9][0-9]?|IV([1-9]|[1-5][0-9]|6[0-3])|KA2[7-8]|KW([1-9]|1[0-7])|PA([2-4][0-9]|6[0-9]|7[0-8])|PH(19|2[0-6]|3[0-9]|40|4[2-4]|49|50))\\s?\\d[A-Z]{2}$/gi";

        private string[] scotlandWalesNorthirelandPostcodes =
        {
            "ZE", "KW", "IV", "HS", "PH", "AB", "DD", "PA", "FK",
            "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9",
            "KY", "KA", "EH", "ML","LL", "SY", "LD", "SA", "CF", "BT",
        };

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
