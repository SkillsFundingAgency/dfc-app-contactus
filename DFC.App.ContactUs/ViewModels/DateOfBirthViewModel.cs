using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DateOfBirthViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "Please enter numbers 1 to 31 for day of birth";
        private const string InvalidCharactersMonthValidationError = "Please enter numbers 1 to 12 for month of birth";
        private const string InvalidCharactersYearValidationError = "Please enter numbers for a 4 digit year of birth after 1900";

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
