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

        public string? To { get; set; }

        public string? From { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
