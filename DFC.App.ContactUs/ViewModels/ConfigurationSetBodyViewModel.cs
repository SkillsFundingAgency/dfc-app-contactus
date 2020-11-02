using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class ConfigurationSetBodyViewModel
    {
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Lines Open Text")]
        public string? LinesOpenText { get; set; }
    }
}
