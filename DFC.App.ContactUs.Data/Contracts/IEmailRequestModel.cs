using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEmailRequestModel
    {
        public string? GivenName { get; set; }

        public string? FamilyName { get; set; }

        public string? Subject { get; set; }

        public string? FromEmailAddress { get; set; }

        public string? ToEmailAddress { get; set; }

        public string? Body { get; set; }

        public string? BodyNoHtml { get; set; }

        public Dictionary<string, string?> TokenValueMappings { get; }
    }
}