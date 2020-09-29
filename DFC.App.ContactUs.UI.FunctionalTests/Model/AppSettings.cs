// <copyright file="AppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Config;

namespace DFC.App.ContactUs.Model
{
    internal class AppSettings : IConfiguration
    {
        public ProjectConfiguration ProjectConfiguration { get; set; }

        public TimeoutConfiguration TimeoutConfiguration { get; set; }

        public BrowserStackConfiguration BrowserStackConfiguration { get; set; }

        public EnvironmentConfiguration EnvironmentConfiguration { get; set; }

        public MongoDatabaseConfiguration MongoDatabaseConfiguration { get; set; }

        public BuildConfiguration BuildConfiguration { get; set; }

        public BrowserConfiguration BrowserConfiguration { get; set; }
    }
}
