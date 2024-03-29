﻿using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Controllers
{
    public class HealthController : Controller
    {
        public const string HealthViewCanonicalName = "health";

        private readonly ILogger<HealthController> logger;
        private readonly string resourceName = typeof(Program).Namespace!;

        public HealthController(ILogger<HealthController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("pages/health")]
        public IActionResult HealthView()
        {
            var result = Health();

            return result;
        }

        [HttpGet]
        [Route("health")]
        public IActionResult Health()
        {
            logger.LogInformation($"{nameof(Health)} has been called");

            const string message = "Document store is available";
            logger.LogInformation($"{nameof(Health)} responded with: {resourceName} - healthy");

            var viewModel = CreateHealthViewModel(message);

            return this.NegotiateContentResult(viewModel, viewModel.HealthItems);
        }

        [HttpGet]
        [Route("health/ping")]
        public IActionResult Ping()
        {
            logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }

        private HealthViewModel CreateHealthViewModel(string message)
        {
            return new HealthViewModel
            {
                HealthItems = new List<HealthItemViewModel>
                {
                    new HealthItemViewModel
                    {
                        Service = resourceName,
                        Message = message,
                    },
                },
            };
        }
    }
}