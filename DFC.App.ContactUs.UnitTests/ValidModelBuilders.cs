using DFC.App.ContactUs.Data.Enums;
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
                CallbackDateOptionSelected = CallbackDateOption.TodayPlus1,
                CallbackTimeOptionSelected = CallbackTimeOption.Band3,
                TermsAndConditionsAccepted = true,
                SelectedCategory = Category.Callback,
                MoreDetail = "some more detail",
            };
        }
    }
}
