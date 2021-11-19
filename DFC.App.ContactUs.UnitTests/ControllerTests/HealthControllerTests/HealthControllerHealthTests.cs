using DFC.App.ContactUs.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Category", "Health Controller Unit Tests")]
    public class HealthControllerHealthTests : BaseHealthControllerTests
    {
        [Fact]
        public void HealthControllerHealthReturnsSuccessWhenHealthy()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            // Act
            var result = controller.Health();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var models = Assert.IsAssignableFrom<List<HealthItemViewModel>>(jsonResult.Value);

            models.Count.Should().BeGreaterThan(0);
            models.First().Service.Should().NotBeNullOrWhiteSpace();
            models.First().Message.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }
    }
}
