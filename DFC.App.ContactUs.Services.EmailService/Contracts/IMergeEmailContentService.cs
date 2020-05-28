namespace DFC.App.ContactUs.Services.EmailService.Contracts
{
    public interface IMergeEmailContentService
    {
        string? MergeTemplateBodyWithContent(IEmailRequestModel? emailRequestModel, string? content);
    }
}
