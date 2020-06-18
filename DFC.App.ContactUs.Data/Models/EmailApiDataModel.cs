using DFC.App.ContactUs.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.ContactUs.Data.Models
{
    public class EmailApiDataModel : IApiDataModel
    {
        [JsonProperty("uri")]
        public Uri? Url { get; set; }
    }
}
