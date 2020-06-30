using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DFC.App.ContactUs.ViewModels
{
    public class EnterYourDetailsBodyViewModel
    {
        public const string TermsAndConditionsLabel = "I accept the <a id='TermsAndConditionsLink' class='govuk-link' href='/help/terms-and-conditions' target='_blank'>terms and conditions</a>";
        public const string TermsAndConditionsValidationError = "You must accept our Terms and Conditions";

        public const string CallbackDateOptionValidationError = "Choose a callback date";
        public const string CallbackTimeOptionValidationError = "Choose a callback time";

        private const string RegExForName = "^[a-zA-Z ]+(([',.\\-][a-zA-Z ])?[a-zA-Z ]*)*$";
        private const string RegExForPostcode = "^([bB][fF][pP][oO]\\s{0,1}[0-9]{1,4}|[gG][iI][rR]\\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";
        private const string RegExForEmailAddress = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
        private const string RegExForTelephoneNumber = "^[ +(0-9]?([ \\-()0-9])+([ 0-9])*$";

        private const string StringLengthValidationError = "{0} is limited to between 1 and {1} characters";

        private const string InvalidCharactersValidationError = "{0} is too long or contains invalid characters";

        public static Dictionary<CallbackTimeOption, int> TimeBandStarts
        {
            get
            {
                return new Dictionary<CallbackTimeOption, int>
                {
                    { CallbackTimeOption.Band1, 8 },
                    { CallbackTimeOption.Band2, 10 },
                    { CallbackTimeOption.Band3, 12 },
                    { CallbackTimeOption.Band4, 14 },
                    { CallbackTimeOption.Band5, 16 },
                };
            }
        }

        public static bool FirstDateIsForToday
        {
            get
            {
                return DateTime.Now.Date == DateTime.Today && DateTime.Now.Hour < TimeBandStarts[TimeBandStarts.Keys.Last()];
            }
        }

        public static Dictionary<CallbackTimeOption, bool> DisabledTimeBands
        {
            get
            {
                var disabledTimeBands = new Dictionary<CallbackTimeOption, bool>();
                bool isToday = FirstDateIsForToday;

                for (var i = CallbackTimeOption.Band1; i <= CallbackTimeOption.Band5; i++)
                {
                    disabledTimeBands.Add(i, isToday && DateTime.Now.Hour >= TimeBandStarts[i]);
                }

                return disabledTimeBands;
            }
        }

        public static Dictionary<CallbackDateOption, string> DateLabels
        {
            get
            {
                var dateLabels = new Dictionary<CallbackDateOption, string>();
                var dateValue = DateTime.Today;

                if (!FirstDateIsForToday)
                {
                    dateValue = dateValue.AddDays(1);
                }

                for (var i = CallbackDateOption.Today; i <= CallbackDateOption.TodayPlus5; i++)
                {
                    if (dateValue.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dateValue = dateValue.AddDays(1);
                    }

                    if (dateValue.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dateValue = dateValue.AddDays(1);
                    }

                    string daySuffix = (dateValue.Day % 10) switch
                    {
                        1 => "st",
                        2 => "nd",
                        3 => "rd",
                        _ => "th",
                    };

                    var labelString = dateValue.ToString("dddd d?? MMMM", CultureInfo.InvariantCulture).Replace("??", daySuffix, StringComparison.OrdinalIgnoreCase);

                    dateLabels.Add(i, labelString);

                    dateValue = dateValue.AddDays(1);
                }

                return dateLabels;
            }
        }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Enter your first name")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Enter your last name")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? LastName { get; set; }

        [Display(Name = "Email address")]
        [RequiredWhenTrue(nameof(EmailAddressIsRequired), ErrorMessage = "Enter your {0}")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForEmailAddress, ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        [DataType("EmailAddress")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Telephone number")]
        [RequiredWhenTrue(nameof(TelephoneNumberIsRequired), ErrorMessage = "Enter your {0}")]
        [StringLength(20, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForTelephoneNumber, ErrorMessage = "{0} requires numbers only")]
        [DataType("TelephoneNumber")]
        public string? TelephoneNumber { get; set; }

        [Display(Name = "Date of birth", Description = "For example, 31 3 1980")]
        [DateOfBirthValidation(13, "1900-01-01", ErrorMessage = "{0}")]
        [DataType("DateTimeEditor")]
        public DateOfBirthViewModel? DateOfBirth { get; set; } = new DateOfBirthViewModel();

        [Display(Name = "Postcode", Description = "For example, SW1A 1AA")]
        [Required(ErrorMessage = "Enter your postcode")]
        [StringLength(8, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForPostcode, ErrorMessage = "{0} must be an English or BFPO postcode")]
        [DataType("Postcode")]
        public string? Postcode { get; set; }

        [Display(Name = "Pick a day for us to call you")]
        [RequiredWhenTrue(nameof(IsCallback), ErrorMessage = CallbackDateOptionValidationError)]
        [EnumDataType(typeof(CallbackDateOption))]
        public CallbackDateOption? CallbackDateOptionSelected { get; set; }

        [Display(Name = "Pick a time for us to call you")]
        [RequiredWhenTrue(nameof(IsCallback), ErrorMessage = CallbackTimeOptionValidationError)]
        [CallbackTimeOptionValidator(nameof(CallbackDateOptionSelected), ErrorMessage = "This time period has expired. Choose a different time")]
        [EnumDataType(typeof(CallbackTimeOption))]
        public CallbackTimeOption? CallbackTimeOptionSelected { get; set; }

        [Display(Name = "Terms and conditions")]
        [Compare(nameof(IsTrue), ErrorMessage = TermsAndConditionsValidationError)]
        public bool TermsAndConditionsAccepted { get; set; }

        public bool IsTrue => true;

        public string? MoreDetail { get; set; }

        public Category SelectedCategory { get; set; }

        public bool IsCallback { get; set; }

        public bool EmailAddressIsRequired
        {
            get
            {
                return !IsCallback;
            }
        }

        public bool TelephoneNumberIsRequired
        {
            get
            {
                return IsCallback;
            }
        }
    }
}
