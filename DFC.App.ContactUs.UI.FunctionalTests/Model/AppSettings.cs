// <copyright file="AppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Config;
using DFC.TestAutomation.UI.Helpers;

namespace DFC.App.ContactUs.Model
{
    internal class AppSettings : IConfiguration
    {
        public ProjectConfig ProjectConfig { get; set; }

        public TimeOutConfig TimeOutConfig { get; set; }

        public BrowserStackSetting BrowserStackConfig { get; set; }

        public EnvironmentConfig EnvironmentConfig { get; set; }

        public TestExecutionConfig TestExecutionConfig { get; set; }

        public MongoDbConfig MongoDbConfig { get; set; }
    }
}
