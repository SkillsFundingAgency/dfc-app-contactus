using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    public class BaseHomeController
    {
        protected HomeController BuildHomeController()
        {
            var controller = new HomeController()
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext(),
                },
            };

            return controller;
        }
    }
}
