namespace DFC.App.ContactUs.Services.EmailService.Models
{
    public class SendGridSettings
    {
        public string? ApiKey { get; set; }

        public int DefaultNumberOfRetries { get; set; } = 2;

        public int DefaultMinimumBackOff { get; set; } = 2;

        public int DeltaBackOff { get; set; } = 3;

        public int DefaultMaximumBackOff { get; set; } = 3;
    }
}
