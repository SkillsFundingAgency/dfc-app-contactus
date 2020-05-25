using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Models;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HomeBodyViewModel
    {
        public const string SelectedOptionValidationError = "Choose an option";

        public ServiceOpenDetailModel ServiceOpenDetailModel { get; set; } = new ServiceOpenDetailModel();

        [Required(ErrorMessage = SelectedOptionValidationError)]
        [Range((int)HomeOption.Webchat, (int)HomeOption.Sendletter, ErrorMessage = SelectedOptionValidationError)]
        [EnumDataType(typeof(HomeOption))]
        public HomeOption? SelectedOption { get; set; }
    }
}
