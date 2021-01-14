using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEmailRequestModel
    {
        public string? GivenName { get; set; }

        public string? FamilyName { get; set; }

        public string? TelephoneNumber { get; set; }

        public string? Subject { get; set; }

        public string? FromEmailAddress { get; set; }

        public string? ToEmailAddress { get; set; }

        public bool IsCallBack { get; set; }

        public Dictionary<string, dynamic> PersonalisationMappings { get; }
    }
}