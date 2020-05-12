using DFC.App.ContactUs.Models;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HomeBodyViewModel
    {
        private const string SelectedOptionValidationError = "Choose an option";

        public enum SelectOption
        {
            None,
            Webchat,
            SendAMessage,
            Callback,
            Sendletter,
        }

        public ServiceOpenDetailModel ServiceOpenDetailModel { get; set; } = new ServiceOpenDetailModel();

        [Required(ErrorMessage = SelectedOptionValidationError)]
        [Range((int)SelectOption.Webchat, (int)SelectOption.Sendletter, ErrorMessage = SelectedOptionValidationError)]
        public SelectOption SelectedOption { get; set; } = SelectOption.None;
    }
}
