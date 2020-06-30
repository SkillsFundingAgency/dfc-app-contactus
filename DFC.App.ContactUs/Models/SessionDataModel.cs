using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Enums;

namespace DFC.App.ContactUs.Models
{
    public class SessionDataModel
    {
        public Category Category { get; set; }

        public string? MoreDetail { get; set; }

        public bool IsCallback { get; set; }
    }
}
