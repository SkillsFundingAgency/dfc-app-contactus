using System;
using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests;

[Trait("Category", "Home Controller Unit Tests")]
public class HomeControllerTests
{
    [Fact]
    public void HomeControllerWithNullContentIdThrows()
    {
        // Arrange
        var logger = A.Fake<ILogger<HomeController>>();
        var fakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
        var fakeStaticContentDocumentService = A.Fake<IDocumentService<StaticContentItemModel>>();
        var fakeClientApiOptions = new CmsApiClientOptions
        {
            ContentIds = null,
        };
        var fakeSharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new HomeController(logger, fakeSessionStateService, fakeSharedContentRedisInterface));

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'ContentIds')", ex.Message);
    }
}