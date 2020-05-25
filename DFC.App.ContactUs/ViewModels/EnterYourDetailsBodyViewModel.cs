using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class EnterYourDetailsBodyViewModel
    {
        public const string IsRequiredValidationError = "Enter your {0}";
        public const string DateOfBirthRequiredValidationError = "{0}";
        public const string CallbackDateTimeRequiredValidationError = "Enter when you want us to contact you";

        private const string RegExForName = "^[a-zA-Z ]+(([',.\\-][a-zA-Z ])?[a-zA-Z ]*)*$";
        private const string RegExForPostcode = "([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\\s?[0-9][A-Za-z]{2})";
        private const string RegExForEmailAddress = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
        private const string RegExForTelephoneNumber = "^[ +(0-9]?([ \\-()0-9])+([ 0-9])*$";

        private const string StringLengthValidationError = "{0} is limited to between 1 and {1} characters";

        private const string InvalidCharactersValidationError = "{0} contains invalid characters";
        private const string EmailAddressInvalidCharactersValidationError = "Enter a valid {0}";
        private const string TelephoneNumberInvalidCharactersValidationError = "{0} requires numbers only";
        private const string PostcodeInvalidCharactersValidationError = "{0} must be an English postcode";
        private const string TermsAndConditionsAcceptedValidationError = "Please tick the {0}";

        [Display(Name = "First name")]
        [Required(ErrorMessage = IsRequiredValidationError)]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? FirstName { get; set; }

        [Display(Name = "Family name")]
        [Required(ErrorMessage = IsRequiredValidationError)]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? FamilyName { get; set; }

        [Display(Name = "Email address")]
        [RequiredWhenTrue(nameof(EmailAddressIsRequired), ErrorMessage = IsRequiredValidationError)]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForEmailAddress, ErrorMessage = EmailAddressInvalidCharactersValidationError)]
        [DataType("EmailAddress")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Telephone number")]
        [RequiredWhenTrue(nameof(TelephoneNumberIsRequired), ErrorMessage = IsRequiredValidationError)]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForTelephoneNumber, ErrorMessage = TelephoneNumberInvalidCharactersValidationError)]
        [DataType("TelephoneNumber")]
        public string? TelephoneNumber { get; set; }

        [Display(Name = "Date of birth", Description = "Date of birth")]
        [DateOfBirthValidation(13, ErrorMessage = DateOfBirthRequiredValidationError)]
        [DataType("DateEditor")]
        public DateOfBirthViewModel? DateOfBirth { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = IsRequiredValidationError)]
        [StringLength(8, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForPostcode, ErrorMessage = PostcodeInvalidCharactersValidationError)]
        public string? Postcode { get; set; }

        [Display(Name = "When do you want us to contact you?")]
        [CallbackDateTimeValidation(3, nameof(CallbackDateTimeIsRequired), ErrorMessage = CallbackDateTimeRequiredValidationError)]
        [DataType("DateTimeEditor")]
        public CallbackDateTimeViewModel? CallbackDateTime { get; set; }

        [Display(Name = "Terms and conditions", Prompt = "I accept the terms and conditions and I am 13 or over.")]
        [Compare(nameof(IsTrue), ErrorMessage = TermsAndConditionsAcceptedValidationError)]
        public bool TermsAndConditionsAccepted { get; set; }

        public bool IsTrue => true;

        public Category SelectedCategory { get; set; }

        public bool EmailAddressIsRequired
        {
            get
            {
                return SelectedCategory != Category.Callback;
            }
        }

        public bool TelephoneNumberIsRequired
        {
            get
            {
                return SelectedCategory == Category.Callback;
            }
        }

        public bool CallbackDateTimeIsRequired
        {
            get
            {
                return SelectedCategory == Category.Callback;
            }
        }
    }
}
