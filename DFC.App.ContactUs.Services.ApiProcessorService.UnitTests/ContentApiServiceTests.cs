using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Services.CmsApiProcessorService;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.ApiProcessorService.UnitTests
{
    public class ContentApiServiceTests
    {
        private readonly IApiDataProcessorService apiDataProcessorService = A.Fake<IApiDataProcessorService>();

        [Fact]
        public async Task ContentApiServiceGetAllReturnsList()
        {
            var fakeApiModelList = new List<EmailApiDataModel> { new EmailApiDataModel { Body = "<h1>Test</h1>" } };

            //Arrange
            var serviceToTest = new ContentApiService<EmailApiDataModel>(apiDataProcessorService, A.Fake<HttpClient>());
            A.CallTo(() => apiDataProcessorService.GetAsync<IEnumerable<EmailApiDataModel>>(A<HttpClient>.Ignored, A<string>.Ignored)).Returns(fakeApiModelList);

            //Act
            var result = await serviceToTest.GetAll("email").ConfigureAwait(false);

            //Assert
            Assert.Equal(fakeApiModelList, result);
        }

        [Fact]
        public async Task ContentApiServiceGetByUrlReturnsItem()
        {
            var fakeApiModel = new EmailApiDataModel { Body = "<h1>Test</h1>" };

            //Arrange
            var serviceToTest = new ContentApiService<EmailApiDataModel>(apiDataProcessorService, A.Fake<HttpClient>());
            A.CallTo(() => apiDataProcessorService.GetAsync<EmailApiDataModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(fakeApiModel);

            //Act
            var result = await serviceToTest.GetById(new System.Uri($"https://somewhere.com/email/{Guid.NewGuid()}")).ConfigureAwait(false);

            //Assert
            Assert.Equal(fakeApiModel, result);
        }
    }
}
