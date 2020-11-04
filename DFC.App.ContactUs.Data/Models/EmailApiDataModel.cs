<<<<<<< HEAD
﻿using DFC.Content.Pkg.Netcore.Data.Contracts;
using Newtonsoft.Json;
using System;
=======
﻿using DFC.Content.Pkg.Netcore.Data.Models;
>>>>>>> story/DFCC-1169-refresh-nugets
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
<<<<<<< HEAD
    public class EmailApiDataModel : IApiDataModel
    {
        [JsonProperty("skos__prefLabel")]
        public string? Title { get; set; }

        [JsonProperty("uri")]
        public Uri? Url { get; set; }

=======
    public class EmailApiDataModel : BaseContentItemModel
    {
>>>>>>> story/DFCC-1169-refresh-nugets
        public string? To { get; set; }

        public string? From { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
