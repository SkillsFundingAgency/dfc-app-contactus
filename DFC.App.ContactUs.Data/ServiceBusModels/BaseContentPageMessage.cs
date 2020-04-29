using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Data.ServiceBusModels
{
    public class BaseContentPageMessage
    {
        [Required]
        public Guid ContentPageId { get; set; }
    }
}
