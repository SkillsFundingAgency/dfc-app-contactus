using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailModel : RequestTrace, IDocumentModel
    {
        public const string DefaultPartitionKey = "email";

        public EmailModel()
        {
            PartitionKey = DefaultPartitionKey;
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get; set; }

        public DateTime LastCached { get; set; } = DateTime.UtcNow;
    }
}
