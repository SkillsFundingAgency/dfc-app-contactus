using System;

namespace DFC.App.ContactUs.ViewModels
{
    public class ChatViewBodyModel
    {
        public Uri? ChatUrl { get; set; }

        public string? PhoneNumber { get; set; }

        public bool ShowWebchatIframe { get; set; }
    }
}