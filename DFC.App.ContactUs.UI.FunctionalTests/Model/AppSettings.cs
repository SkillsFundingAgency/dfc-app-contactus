// <copyright file="AppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Settings;
using System;

namespace DFC.App.ContactUs.Model
{
    internal class AppSettings : IAppSettings
    {
        public string AppName { get; set; }

        public Uri AppUrl { get; set; }

        public bool TakeScreenshots { get; set; }
    }
}
