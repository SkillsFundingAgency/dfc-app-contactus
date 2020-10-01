// <copyright file="ContactUsSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Settings;

namespace DFC.App.ContactUs.Model
{
    internal class ContactUsSettings : IAppSettings
    {
        public string AppName { get; set; }

        public string AppUrl { get; set; }

        public bool TakeScreenshots { get; set; }
    }
}
