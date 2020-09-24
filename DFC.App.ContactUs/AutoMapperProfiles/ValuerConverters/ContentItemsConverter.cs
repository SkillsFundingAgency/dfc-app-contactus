using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters
{
    public class ContentItemsConverter : IValueConverter<IList<ContentItemModel>?, HtmlString?>
    {
        private readonly Dictionary<int, string> columnWidthClasses = new Dictionary<int, string>
        {
            { 25, "govuk-grid-column-one-quarter" },
            { 33, "govuk-grid-column-one-third" },
            { 50, "govuk-grid-column-one-half" },
            { 66, "govuk-grid-column-two-thirds" },
            { 75, "govuk-grid-column-three-quarters" },
        };

        private readonly Dictionary<int, string> justifyClasses = new Dictionary<int, string>
        {
            { 2, "govuk-grid-column-center" },
            { 3, "govuk-grid-column-right" },
        };

        public HtmlString? Convert(IList<ContentItemModel>? sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }

            var result = new StringBuilder();
            foreach (var contactUsApiContentItemModel in sourceMember.OrderBy(o => o.Ordinal))
            {
                var itemClass = "govuk-grid-column-full";

                if (columnWidthClasses.Keys.Contains(contactUsApiContentItemModel.Width))
                {
                    itemClass = columnWidthClasses[contactUsApiContentItemModel.Width];
                }

                if (justifyClasses.Keys.Contains(contactUsApiContentItemModel.Justify))
                {
                    itemClass += " " + justifyClasses[contactUsApiContentItemModel.Justify];
                }

                result.Append($"<div class=\"{itemClass}\">");
                result.Append(contactUsApiContentItemModel.Content);
                result.Append("</div>");
            }

            return new HtmlString(result.ToString());
        }
    }
}
