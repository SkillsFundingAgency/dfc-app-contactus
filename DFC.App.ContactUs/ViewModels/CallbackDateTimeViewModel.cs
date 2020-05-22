using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class CallbackDateTimeViewModel : DateViewModel
    {
        private const string NumberRangeValidationError = "Callback {0} must be between {1} and {2}";

        public CallbackDateTimeViewModel() : base()
        {
            IncludeTimeValue = true;
        }

        public CallbackDateTimeViewModel(DateTime dateTime) : base(dateTime)
        {
            IncludeTimeValue = true;
        }

        [Range(1, 31, ErrorMessage = NumberRangeValidationError)]
        public override int? Day { get; set; }

        [Range(1, 12, ErrorMessage = NumberRangeValidationError)]
        public override int? Month { get; set; }

        public override int? Year { get; set; }

        [Range(0, 23, ErrorMessage = NumberRangeValidationError)]
        public override int? Hour { get; set; }

        [Range(0, 59, ErrorMessage = NumberRangeValidationError)]
        public override int? Minute { get; set; }

        public override bool IncludeTimeValue { get; set; }
    }
}
