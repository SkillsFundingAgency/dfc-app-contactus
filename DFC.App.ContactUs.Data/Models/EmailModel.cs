using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
using System;

namespace DFC.App.ContactUs.Data.Models
{
    public class EmailModel : RequestTrace, IDocumentModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public string? Body { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get => "email"; set => throw new NotImplementedException(); }
    }
}
