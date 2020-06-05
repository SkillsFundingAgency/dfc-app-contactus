using DFC.App.ContactUs.Services.PageService.Contracts;
using Newtonsoft.Json;
using System;

namespace DFC.App.ContactUs.Services.EventProcessorService.Contracts
{
    public interface IEventDataModel : IServiceDataModel
    {
        [JsonProperty(Order = -10)]
        Guid? Version { get; set; }
    }
}
