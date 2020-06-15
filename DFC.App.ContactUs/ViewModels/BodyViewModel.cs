using Microsoft.AspNetCore.Html;

namespace DFC.App.ContactUs.ViewModels
{
    public class BodyViewModel
    {
        public HtmlString? Content { get; set; } = new HtmlString("Unknown content");
    }
}
