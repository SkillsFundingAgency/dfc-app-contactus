namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IMergeEmailContentService
    {
        string? MergeTemplateBodyWithContent(IEmailRequestModel? emailRequestModel, string? content);
    }
}
