using DFC.App.ContactUs.Data;
using System;

namespace DFC.App.ContactUs.ViewModels
{
    public class ChatViewBodyModel
    {
        public Uri? ChatUrl { get; set; }

        public string? PhoneNumber { get; set; } = Constants.DefaultPhoneNumber;

        public string? HowCanWeHelpLink { get; set; }

        public bool ShowWebchatIframe { get; set; }
    }
}