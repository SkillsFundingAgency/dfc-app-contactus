using DFC.App.ContactUs.CustomControls.GenericCheckbox.Templates;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.CustomControls.GenericCheckbox.TagHelpers
{
    [ExcludeFromCodeCoverage]
    public class GenericCheckboxTagHelper : TagHelper
    {
        public string? Name { get; set; }

        public string? Label { get; set; }

        public string? Class { get; set; }

        public bool Value { get; set; }

        public bool IsDisabled { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput? output)
        {
            _ = output ?? throw new ArgumentNullException(nameof(output));
            _ = Name ?? throw new NullReferenceException(nameof(Name));
            _ = Label ?? throw new NullReferenceException(nameof(Label));
            _ = Class ?? throw new NullReferenceException(nameof(Class));

            output.TagName = string.Empty;
            var template = TemplateGenerator.Generate(Name, Label, Class, Value, IsDisabled);
            output.Content.SetHtmlContent(template);
        }
    }
}
