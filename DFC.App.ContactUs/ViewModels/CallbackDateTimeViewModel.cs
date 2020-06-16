using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class CallbackDateTimeViewModel : DateViewModel
    {
        private const string InvalidCharactersDayValidationError = "Enter a valid call back date. Day must be a number from 1 to 31";
        private const string InvalidCharactersMonthValidationError = "Enter a valid call back date. Month must be a number from 1 to 12";
        private const string InvalidCharactersYearValidationError = "Enter a valid call back date. Year must be a 4 digit number after 1900";
        private const string InvalidCharactersHourValidationError = "Enter a valid call back time. Hour must be a number from 0 to 23";
        private const string InvalidCharactersMinuteValidationError = "Enter a valid call back time. Minute must be a number from 0 to 59";

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
