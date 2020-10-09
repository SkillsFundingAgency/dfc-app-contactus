using System.Collections.Generic;

namespace DFC.App.ContactUs.ViewModels
{
    public class IndexViewModel
    {
        public string? LocalPath { get; set; }

        public List<IndexDocumentViewModel>? Documents { get; set; }
    }
}
