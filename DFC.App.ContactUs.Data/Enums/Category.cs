﻿using System.ComponentModel;

namespace DFC.App.ContactUs.Data.Enums
{
    public enum Category
    {
        [Description("None")]
        None,
        [Description("Careers advice and guidance")]
        AdviceGuidance,
        [Description("Courses, training and qualifications")]
        Courses,
        [Description("Problems using the website")]
        Website,
        [Description("Give feedback")]
        Feedback,
        [Description("Other")]
        Other,
        [Description("Callback request")]
        Callback,
    }
}
