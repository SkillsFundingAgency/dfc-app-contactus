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

        public string? CallbackDateTime { get; set; }

        public string? Subject { get; set; }

        public string? FromEmailAddress { get; set; }

        public string? ToEmailAddress { get; set; }

        public bool IsCallBack { get; set; }

        public Dictionary<string, dynamic> PersonalisationMappings
        {
            get
            {
                return new Dictionary<string, dynamic>
                {
                    { nameof(Subject), Subject },
                    { nameof(GivenName), GivenName },
                    { nameof(FamilyName), FamilyName },
                    { nameof(FromEmailAddress), FromEmailAddress },
                    { nameof(TelephoneNumber), TelephoneNumber },
                    { nameof(DateOfBirth), DateOfBirth.HasValue ? DateOfBirth.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null },
                    { nameof(Postcode), Postcode },
                    { nameof(Query), Query },
                    { nameof(CallbackDateTime), CallbackDateTime },
                };
            }
        }
    }
}
