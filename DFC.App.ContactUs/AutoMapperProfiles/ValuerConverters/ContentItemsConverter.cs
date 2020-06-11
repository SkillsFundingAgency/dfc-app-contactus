using AutoMapper;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters
{
    [ExcludeFromCodeCoverage]
    public class ContentItemsConverter : IValueConverter<IList<ContactUsApiContentItemModel>, string?>
    {
        private readonly Dictionary<int, string> columnWidthClasses = new Dictionary<int, string>
        {
            { 25, "govuk-grid-column-one-quarter" },
            { 33, "govuk-grid-column-one-third" },
            { 50, "govuk-grid-column-one-half" },
            { 66, "govuk-grid-column-two-thirds" },
            { 75, "govuk-grid-column-three-quarters" },
        };

        public string? Convert(IList<ContactUsApiContentItemModel> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }

            var result = new StringBuilder();
            var rowNumbers = (from a in sourceMember select a.Row).Distinct().ToList();
            foreach (var rowNumber in rowNumbers)
            {
                result.Append("<div class='govuk-grid-row'>");

                foreach (var contactUsApiContentItemModel in sourceMember.Where(w => w.Row == rowNumber).OrderBy(o => o.Column))
                {
                    var columnWidthClass = "govuk-grid-column-full";

                    if (columnWidthClasses.Keys.Contains(contactUsApiContentItemModel.Width))
                    {
                        columnWidthClass = columnWidthClasses[contactUsApiContentItemModel.Width];
                    }

                    result.Append($"<div class='{columnWidthClass}'>");
                    result.Append(contactUsApiContentItemModel.Content);
                    result.Append("</div>");
                }

                result.Append("</div>");
            }

            return result.ToString();
        }
    }
}
