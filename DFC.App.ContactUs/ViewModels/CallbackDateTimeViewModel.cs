﻿using DFC.App.ContactUs.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class CallbackDateTimeViewModel : DateViewModel
    {
        private const string IsRequiredValidationError = "Enter callback {0}";

        private const string NumberRangeValidationError = "Callback {0} must be between {1} and {2}";

        private const string ServiceOpenHoursRangeValidationError = "Service opening hours between {0} and {1}";

        public CallbackDateTimeViewModel() : base()
        {
            IncludeTimeValue = true;
        }

        public CallbackDateTimeViewModel(DateTime dateTime) : base(dateTime)
        {
            IncludeTimeValue = true;
        }

        [Required(ErrorMessage = IsRequiredValidationError)]
        [Range(1, 31, ErrorMessage = NumberRangeValidationError)]
        public override int? Day { get; set; }

        [Required(ErrorMessage = IsRequiredValidationError)]
        [Range(1, 12, ErrorMessage = NumberRangeValidationError)]
        public override int? Month { get; set; }

        [Required(ErrorMessage = IsRequiredValidationError)]
        public override int? Year { get; set; }

        [Required(ErrorMessage = IsRequiredValidationError)]
        [Range(0, 23, ErrorMessage = NumberRangeValidationError)]
        [ServiceOpenHoursRange(nameof(Minute), ErrorMessage = ServiceOpenHoursRangeValidationError)]
        public override int? Hour { get; set; }

        [Required(ErrorMessage = IsRequiredValidationError)]
        [Range(0, 59, ErrorMessage = NumberRangeValidationError)]
        public override int? Minute { get; set; }

        public override bool IncludeTimeValue { get; set; }
    }
}
