using DFC.App.ContactUs.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HomeBodyViewModel
    {
        private const string SelectedOptionValidationError = "Choose an option";

        public enum SelectOption
        {
            [Description("None")]
            None,
            [Description("Speak to an adviser using webchat")]
            Webchat,
            [Description("Send us an online message. We'll email you back")]
            SendAMessage,
            [Description("Ask us to call you back")]
            Callback,
            [Description("Send us a letter")]
            Sendletter,
        }

        public ServiceOpenDetailModel ServiceOpenDetailModel { get; set; } = new ServiceOpenDetailModel();

        [Required(ErrorMessage = SelectedOptionValidationError)]
        [Range((int)SelectOption.Webchat, (int)SelectOption.Sendletter, ErrorMessage = SelectedOptionValidationError)]
        [EnumDataType(typeof(SelectOption))]
        public SelectOption SelectedOption { get; set; } = SelectOption.None;
    }
}
