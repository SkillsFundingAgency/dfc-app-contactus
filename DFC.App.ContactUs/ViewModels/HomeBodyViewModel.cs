using DFC.App.ContactUs.Data;
﻿using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HomeBodyViewModel
    {
        public const string SelectedOptionValidationError = "Choose an option";

        public string? PhoneNumber { get; set; } = Constants.DefaultPhoneNumber;

        public string? ContactUs { get; set; }

        [Required(ErrorMessage = SelectedOptionValidationError)]
        [Range((int)HomeOption.Webchat, (int)HomeOption.Sendletter, ErrorMessage = SelectedOptionValidationError)]
        [EnumDataType(typeof(HomeOption))]
        public HomeOption? SelectedOption { get; set; }
    }
}
