using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Models
{
    public class NotifyOptions
    {
        public string? ApiKey { get; set; }

        public string? ByEmailTemplateId { get; set; }

        public string? CallMeBackTemplateId { get; set; }
    }
}
