using DFC.App.ContactUs.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DFC.App.ContactUs.Models
{
    public class ContactUsEmailRequestModel : IEmailRequestModel
    {
        public string? GivenName { get; set; }

        public string? FamilyName { get; set; }

        public string? TelephoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Postcode { get; set; }

        public string? Query { get; set; }

        public DateTime? CallbackDateTime { get; set; }

        public string? Subject { get; set; }

        public string? FromEmailAddress { get; set; }

        public string? ToEmailAddress { get; set; }

        public string? Body { get; set; }

        public string? BodyNoHtml { get; set; }

        public Dictionary<string, string?> TokenValueMappings
        {
            get
            {
                return new Dictionary<string, string?>
                {
                    { nameof(GivenName), GivenName },
                    { nameof(FamilyName), FamilyName },
                    { nameof(FromEmailAddress), FromEmailAddress },
                    { nameof(TelephoneNumber), TelephoneNumber },
                    { nameof(DateOfBirth), DateOfBirth.HasValue ? DateOfBirth.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null },
                    { nameof(Postcode), Postcode },
                    { nameof(Query), Query },
                    { nameof(CallbackDateTime), CallbackDateTime.HasValue ? CallbackDateTime.Value.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : null },
                };
            }
        }
    }
}
