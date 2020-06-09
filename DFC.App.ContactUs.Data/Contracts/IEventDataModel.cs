using DFC.Compui.Cosmos.Contracts;
using Newtonsoft.Json;
using System;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEventDataModel : IContentPageModel
    {
        [JsonProperty(Order = -10)]
        Guid? Version { get; set; }
    }
}
