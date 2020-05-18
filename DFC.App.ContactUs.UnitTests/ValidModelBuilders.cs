using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using System;

namespace DFC.App.ContactUs.UnitTests
{
    public static class ValidModelBuilders
    {
        public static EnterYourDetailsBodyViewModel BuildValidEnterYourDetailsBodyViewModel()
        {
            return new EnterYourDetailsBodyViewModel
            {
                FirstName = "Mark Anthony",
                FamilyName = "Mark Anthony",
                EmailAddress = "abc@def.com",
                TelephoneNumber = "0123456789",
                DateOfBirth = new DateOfBirthViewModel(DateTime.Today.AddYears(-13)),
                Postcode = "CV1 2AB",
                CallbackDateTime = new CallbackDateTimeViewModel(DateTime.Today.AddDays(1).AddHours(10).AddMinutes(15)),
                TermsAndConditionsAccepted = true,
                SelectedCategory = Category.Callback,
            };
        }
    }
}
