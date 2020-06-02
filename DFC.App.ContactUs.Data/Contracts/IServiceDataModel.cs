using System;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IServiceDataModel : IDataModel
    {
        string CanonicalName { get; set; }

        public IList<string>? AlternativeNames { get; set; }

        Guid? Version { get; set; }
    }
}
