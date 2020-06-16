using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DateOfBirthViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "Enter a valid date of birth. Day must be a number from 1 to 31";
        private const string InvalidCharactersMonthValidationError = "Enter a valid date of birth. Month must be a number from 1 to 12";
        private const string InvalidCharactersYearValidationError = "Enter a valid date of birth. Year must be a 4 digit number after 1900";

        public DateOfBirthViewModel() : base()
        {
        }

        public DateOfBirthViewModel(DateTime dateTime) : base(dateTime)
        {
        }

        [RegularExpression(RegExForDay, ErrorMessage = InvalidCharactersDayValidationError)]
        public override int? Day { get; set; }

        [RegularExpression(RegExForMonth, ErrorMessage = InvalidCharactersMonthValidationError)]
        public override int? Month { get; set; }

        [RegularExpression(RegExForYear, ErrorMessage = InvalidCharactersYearValidationError)]
        public override int? Year { get; set; }

        public override int? Hour { get; set; }

        public override int? Minute { get; set; }

        public override bool IncludeTimeValue { get; set; }
    }
}
