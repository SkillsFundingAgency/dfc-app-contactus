﻿// <copyright file="AppSettings.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
