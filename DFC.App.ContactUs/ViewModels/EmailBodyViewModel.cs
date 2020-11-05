using Microsoft.AspNetCore.Html;

namespace DFC.App.ContactUs.ViewModels
{
    public class EmailBodyViewModel
    {
        public HtmlString? Body { get; set; } = new HtmlString("Unknown content");
    }
}
