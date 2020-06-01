using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DFC.App.ContactUs.CustomControls.GenericCheckbox.Templates
{
    [ExcludeFromCodeCoverage]
    internal static class TemplateGenerator
    {
        public static string Generate(string name, string label, bool value, bool isDisabled)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"<div class=\"govuk-checkboxes\">");

            string checkedValue = value ? "checked" : string.Empty;
            string disabledValue = isDisabled ? "disabled" : string.Empty;
            string nameValue = name;
            string idValue = name;
            string disabledClass = isDisabled ? " disabled" : string.Empty;

            stringBuilder.Append(
                $"<div class=\"govuk-checkboxes__item\">" +
                $"<input class=\"govuk-checkboxes__input\" id=\"{idValue}\" name=\"{nameValue}\" type=\"checkbox\" value=\"true\" {checkedValue} {disabledValue}>" +
                $"<label class=\"checkbox{disabledClass} govuk-label govuk-checkboxes__label\" for=\"{idValue}\">" + label +
                $"</label>" +
                $"</div>");

            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }
    }
}
