using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class CallbackDateTimeViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "Please enter numbers 1 to 31 for a day for us to call you back";
        private const string InvalidCharactersMonthValidationError = "Please enter numbers 1 to 12 for a month for us to call you back";
        private const string InvalidCharactersYearValidationError = "Please enter numbers for a 4 digit year for us to call you back";
        private const string InvalidCharactersHourValidationError = "Please enter numbers 0 to 23 for an hour for us to call you back";
        private const string InvalidCharactersMinuteValidationError = "Please enter numbers 0 to 59 for a minute for us to call you back";

        public CallbackDateTimeViewModel() : base()
        {
            IncludeTimeValue = true;
        }

        public CallbackDateTimeViewModel(DateTime dateTime) : base(dateTime)
        {
            IncludeTimeValue = true;
        }

        [RegularExpression(RegExForDay, ErrorMessage = InvalidCharactersDayValidationError)]
        public override int? Day { get; set; }

        [RegularExpression(RegExForMonth, ErrorMessage = InvalidCharactersMonthValidationError)]
        public override int? Month { get; set; }

        [RegularExpression(RegExForYear, ErrorMessage = InvalidCharactersYearValidationError)]
        public override int? Year { get; set; }

        [RegularExpression(RegExForHour, ErrorMessage = InvalidCharactersHourValidationError)]
        public override int? Hour { get; set; }

        [RegularExpression(RegExForMinute, ErrorMessage = InvalidCharactersMinuteValidationError)]
        public override int? Minute { get; set; }

        public override bool IncludeTimeValue { get; set; }
    }
}
