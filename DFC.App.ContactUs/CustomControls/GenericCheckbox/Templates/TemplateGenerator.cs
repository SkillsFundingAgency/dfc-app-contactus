using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DFC.App.ContactUs.CustomControls.GenericCheckbox.Templates
{
    [ExcludeFromCodeCoverage]
    internal static class TemplateGenerator
    {
        public static string Generate(string name, string label, string className, bool value, bool isDisabled, string? errorMessage, string? compareTo)
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
                $"<input class=\"govuk-checkboxes__input {className}\" id=\"{idValue}\" name=\"{nameValue}\" type=\"checkbox\" value=\"true\" {checkedValue} {disabledValue} data-val=\"true\" ");

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                stringBuilder.Append($"data-val-equalto=\"{errorMessage}\" ");
            }

            if (!string.IsNullOrWhiteSpace(compareTo))
            {
                stringBuilder.Append($"data-val-equalto-other=\"{compareTo}\" ");
            }

            stringBuilder.Append("/>");

            stringBuilder.Append(
                $"<label class=\"checkbox{disabledClass} govuk-label govuk-checkboxes__label\" for=\"{idValue}\">" + label +
                $"</label>" +
                $"</div>");

            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }
    }
}
