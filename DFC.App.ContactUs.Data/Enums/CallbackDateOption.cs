using System.ComponentModel;

namespace DFC.App.ContactUs.Data.Enums
{
    public enum CallbackDateOption
    {
        [Description("Today")]
        Today,
        [Description("Today + 1")]
        TodayPlus1,
        [Description("Today + 2")]
        TodayPlus2,
        [Description("Today + 3")]
        TodayPlus3,
        [Description("Today + 4")]
        TodayPlus4,
        [Description("Today + 5")]
        TodayPlus5,
    }
}
