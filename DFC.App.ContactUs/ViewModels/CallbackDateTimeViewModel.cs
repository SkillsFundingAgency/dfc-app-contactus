using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class CallbackDateTimeViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "The callback date and time requires numbers for the day from 1 to 31";
        private const string InvalidCharactersMonthValidationError = "The callback date and time requires numbers for the month from 1 to 31";
        private const string InvalidCharactersYearValidationError = "The callback date and time requires a 4 digit number for the year";
        private const string InvalidCharactersHourValidationError = "The callback date and time requires numbers for the hour from 0 to 23";
        private const string InvalidCharactersMinuteValidationError = "The callback date and time requires numbers for the minute from 0 to 59";

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
