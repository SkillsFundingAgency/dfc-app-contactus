using System;

namespace DFC.App.ContactUs.Models
{
    public class ChatOptions
    {
        public bool HideProductionWarning { get; set; } = false;

        public Uri ChatUrl { get; set; } = new Uri("https://smokefree.serco.com/visitor/EntryPage.htm");
    }
}
