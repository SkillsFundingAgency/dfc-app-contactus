// <copyright file="ContactUsConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace DFC.App.ContactUs.Model
{
    internal class ContactUsConfiguration
    {
        public string Browser { get; set; }

        public Uri BaseUrl { get; set; }

        public string BuildNumber { get; set; }

        public string EnvironmentName { get; set; }

        public string ProjectName { get; set; }
    }
}
