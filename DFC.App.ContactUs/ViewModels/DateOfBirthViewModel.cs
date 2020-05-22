﻿using DFC.App.ContactUs.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DateOfBirthViewModel : DateViewModel
    {
        private const string NumberRangeValidationError = "Date of birth {0} must be between {1} and {2}";
        private const string YearRangeValidationError = "Date of birth {0} must be before {1} years ago";

        public DateOfBirthViewModel() : base()
        {
        }

        public DateOfBirthViewModel(DateTime dateTime) : base(dateTime)
        {
        }

        [Range(1, 31, ErrorMessage = NumberRangeValidationError)]
        public override int? Day { get; set; }

        [Range(1, 12, ErrorMessage = NumberRangeValidationError)]
        public override int? Month { get; set; }

        [YearPriorToRange(1900, -13, ErrorMessage = YearRangeValidationError)]
        public override int? Year { get; set; }

        public override int? Hour { get; set; }

        public override int? Minute { get; set; }

        public override bool IncludeTimeValue { get; set; }
    }
}
