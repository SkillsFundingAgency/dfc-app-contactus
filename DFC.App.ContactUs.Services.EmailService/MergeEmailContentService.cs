using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;

namespace DFC.App.ContactUs.Services.EmailService
{
    public class MergeEmailContentService : IMergeEmailContentService
    {
        public string? MergeTemplateBodyWithContent(IEmailRequestModel? emailRequestModel, string? content)
        {
            if (emailRequestModel == null || string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            return TokenReplacement(emailRequestModel, content);
        }

        private static string? TokenReplacement(IEmailRequestModel emailRequestModel, string content)
        {
            var mergedContent = content;

            if (!string.IsNullOrWhiteSpace(mergedContent))
            {
                foreach (var key in emailRequestModel.TokenValueMappings.Keys)
                {
                    mergedContent = mergedContent.Replace($"{{{key}}}", emailRequestModel.TokenValueMappings[key], System.StringComparison.OrdinalIgnoreCase);
                }
            }

            return mergedContent;
        }
    }
}
