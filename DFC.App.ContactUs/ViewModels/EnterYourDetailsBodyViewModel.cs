﻿using DFC.App.ContactUs.Attributes;
using DFC.App.ContactUs.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class EnterYourDetailsBodyViewModel
    {
        public const string TermsAndConditionsLabel = "I accept the <a class='govuk-link' href='/help/terms-and-conditions' target='_blank'>terms and conditions</a> and I am 13 or over.";

        private const string RegExForName = "^[a-zA-Z ]+(([',.\\-][a-zA-Z ])?[a-zA-Z ]*)*$";
        private const string RegExForPostcode = "([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\\s?[0-9][A-Za-z]{2})";
        private const string RegExForEmailAddress = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
        private const string RegExForTelephoneNumber = "^[ +(0-9]?([ \\-()0-9])+([ 0-9])*$";

        private const string StringLengthValidationError = "{0} is limited to between 1 and {1} characters";

        private const string InvalidCharactersValidationError = "{0} contains invalid characters";

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Enter your first name")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? FirstName { get; set; }

        [Display(Name = "Family name")]
        [Required(ErrorMessage = "Enter your family name")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForName, ErrorMessage = InvalidCharactersValidationError)]
        [DataType("PersonName")]
        public string? FamilyName { get; set; }

        [Display(Name = "Email address")]
        [RequiredWhenTrue(nameof(EmailAddressIsRequired), ErrorMessage = "Enter your email address")]
        [StringLength(100, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForEmailAddress, ErrorMessage = "Enter a valid email address")]
        [DataType("EmailAddress")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Telephone number")]
        [RequiredWhenTrue(nameof(TelephoneNumberIsRequired), ErrorMessage = "Enter your telephone number")]
        [StringLength(20, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForTelephoneNumber, ErrorMessage = "{0} requires numbers only")]
        [DataType("TelephoneNumber")]
        public string? TelephoneNumber { get; set; }

        [Display(Name = "Date of birth", Description = "Date of birth")]
        [DateOfBirthValidation(13, ErrorMessage = "{0}")]
        [DataType("DateTimeEditor")]
        public DateOfBirthViewModel? DateOfBirth { get; set; } = new DateOfBirthViewModel();

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "Enter your postcode")]
        [StringLength(8, ErrorMessage = StringLengthValidationError)]
        [RegularExpression(RegExForPostcode, ErrorMessage = "{0} must be an English postcode")]
        [DataType("Postcode")]
        public string? Postcode { get; set; }

        [Display(Name = "When do you want us to contact you?")]
        [CallbackDateTimeValidation(3, nameof(CallbackDateTimeIsRequired), ErrorMessage = "Enter when you want us to contact you")]
        [DataType("DateTimeEditor")]
        public CallbackDateTimeViewModel? CallbackDateTime { get; set; } = new CallbackDateTimeViewModel();

        [Display(Name = "Terms and conditions")]
        [Compare(nameof(IsTrue), ErrorMessage = "Please tick the terms and conditions")]
        public bool TermsAndConditionsAccepted { get; set; }

        public bool IsTrue => true;

        public string? MoreDetail { get; set; }

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
