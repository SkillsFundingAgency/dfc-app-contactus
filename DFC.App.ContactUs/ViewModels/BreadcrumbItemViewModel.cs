using Newtonsoft.Json;

namespace DFC.App.ContactUs.ViewModels
{
    public class BreadcrumbItemViewModel
    {
        public string? Route { get; set; }

        public string? Title { get; set; }

        [JsonIgnore]
        public bool AddHyperlink { get; set; } = true;
    }
}
