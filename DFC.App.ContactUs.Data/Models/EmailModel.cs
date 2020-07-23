using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Telemetry.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class EmailModel : RequestTrace, IDocumentModel, IContentItemModel
    {
        public EmailModel()
        {
            PartitionKey = "email";
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        public string? Body { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get; set; }

        public Guid? ItemId { get; set; }

        public IContentLinks? ContentLinks { get; set; }

        public IList<IContentItemModel>? ContentItems { get; set; }
    }
}
