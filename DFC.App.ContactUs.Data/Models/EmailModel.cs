using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailModel : RequestTrace, IDocumentModel
    {
        public const string DefaultPartitionKey = "email";

        [JsonProperty("id")]
        [Required]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        [Required]
        public Uri? Url { get; set; }

        [Required]
        public string? Body { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get; set; } = DefaultPartitionKey;

        public DateTime LastCached { get; set; } = DateTime.UtcNow;
    }
}
