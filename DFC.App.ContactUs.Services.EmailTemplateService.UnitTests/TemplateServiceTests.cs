using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.EmailTemplateService.UnitTests
{
    public class TemplateServiceTests
    {
        private readonly ILogger<TemplateService> logger = A.Fake<ILogger<TemplateService>>();

        [Fact]
        public async Task TemplateServiceGetReturnsSuccess()
        {
            // arrange
            const string templateName = "CallbackTemplate";

            var templateService = new TemplateService(logger);

            // act
            var result = await templateService.GetTemplateByNameAsync(templateName).ConfigureAwait(false);

            // assert
            Assert.NotNull(result);
        }

        [Fact(Skip = "This will be require for the email template retrieval service coming in a later sprint - from shared content app?")]
        public async Task TemplateGetReturnsNullWhenMissingTemplate()
        {
            // arrange
            const string? templateName = null;

            var templateService = new TemplateService(logger);

            // act
            var result = await templateService.GetTemplateByNameAsync(templateName).ConfigureAwait(false);

            // assert
            Assert.Null(result);
        }
    }
}
