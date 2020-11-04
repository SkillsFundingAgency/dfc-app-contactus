using DFC.Compui.Cosmos.Contracts;
<<<<<<< HEAD
using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
=======
>>>>>>> story/DFCC-1169-refresh-nugets
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
<<<<<<< HEAD
    public class EmailModel : RequestTrace, IDocumentModel
    {
        public const string DefaultPartitionKey = "email";

        [JsonProperty("id")]
        [Required]
        public Guid Id { get; set; }
=======
    public class EmailModel : DocumentModel
    {
        public const string DefaultPartitionKey = "email";

        public override string? PartitionKey { get; set; } = DefaultPartitionKey;
>>>>>>> story/DFCC-1169-refresh-nugets

        public string? Title { get; set; }

        [Required]
        public Uri? Url { get; set; }

        [Required]
        public string? Body { get; set; }

<<<<<<< HEAD
        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get; set; } = DefaultPartitionKey;
=======
        public DateTime LastReviewed { get; set; }

        public DateTime CreatedDate { get; set; }
>>>>>>> story/DFCC-1169-refresh-nugets

        public DateTime LastCached { get; set; } = DateTime.UtcNow;
    }
}
