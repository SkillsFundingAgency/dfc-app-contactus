using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DateOfBirthViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "Your date of birth requires numbers for the day from 1 to 31";
        private const string InvalidCharactersMonthValidationError = "Your date of birth requires numbers for the month from 1 to 12";
        private const string InvalidCharactersYearValidationError = "Your date of birth requires a 4 digit number for the year";

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
