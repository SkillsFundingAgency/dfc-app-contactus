using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Data.Models
{
    public class ContentPageModel : DFC.Compui.Cosmos.Models.ContentPageModel
    {
        [Required]
        [JsonProperty(Order = -10)]
        public override string PartitionKey => "static-page";
    }
}
