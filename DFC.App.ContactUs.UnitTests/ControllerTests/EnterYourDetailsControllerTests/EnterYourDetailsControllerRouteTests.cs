using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.AreaRoutingService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies;
using DFC.App.ContactUs.Services.EmailService.Contracts;
using DFC.App.ContactUs.Services.EmailTemplateService.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerRouteTests : BaseEnterYourDetailsControllerTests
    {
        private readonly ILogger<EnterYourDetailsController> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IRoutingService routingService;
        private readonly ISendGridEmailService<ContactUsEmailRequestModel> sendGridEmialService;
        private readonly FamApiRoutingOptions famApiRoutingOptions;
        private readonly ITemplateService templateService;

        public EnterYourDetailsControllerRouteTests()
        {
            logger = A.Fake<ILogger<EnterYourDetailsController>>();
            mapper = A.Fake<AutoMapper.IMapper>();
            routingService = A.Fake<IRoutingService>();
            sendGridEmialService = A.Fake<ISendGridEmailService<ContactUsEmailRequestModel>>();
            famApiRoutingOptions = A.Fake<FamApiRoutingOptions>();
            templateService = A.Fake<ITemplateService>();
        }

        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}",  nameof(EnterYourDetailsController.EnterYourDetailsView) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/htmlhead",  nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/breadcrumb", nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/body", nameof(EnterYourDetailsController.EnterYourDetailsBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public void EnterYourDetailsControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private IActionResult RunControllerAction(EnterYourDetailsController controller, string actionName)
        {
            return actionName switch
            {
                nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) => controller.EnterYourDetailsHtmlHead(),
                nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) => controller.EnterYourDetailsBreadcrumb(),
                _ => controller.EnterYourDetailsBody(),
            };
        }

        private EnterYourDetailsController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new EnterYourDetailsController(logger, mapper, routingService, sendGridEmialService, famApiRoutingOptions, templateService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}