using System;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ICachedModel
    {
        string? Title { get; set; }

        Uri? Url { get; set; }
    }
}
