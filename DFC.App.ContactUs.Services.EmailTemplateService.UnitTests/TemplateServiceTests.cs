using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.EmailTemplateService.UnitTests
{
    public class TemplateServiceTests
    {
        private readonly ILogger<TemplateService> logger = A.Fake<ILogger<TemplateService>>();
        private readonly IDocumentService<EmailModel> fakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();

        [Fact]
        public async Task TemplateServiceGetReturnsSuccess()
        {
            // arrange
            A.CallTo(() => fakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(new EmailModel() { Body = "<h1>A test bodt</h1>" });
            Guid templateGuid = Guid.Parse("9d50786e-95f2-4b7e-a604-ceaa7a5bc230");

            var templateService = new TemplateService(logger, fakeEmailDocumentService);

            // act
            var result = await templateService.GetTemplateByKeyAsync(templateGuid).ConfigureAwait(false);

            // assert
            Assert.NotNull(result);
        }
    }
}
