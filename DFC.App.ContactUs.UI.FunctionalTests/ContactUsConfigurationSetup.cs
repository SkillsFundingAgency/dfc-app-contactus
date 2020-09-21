// <copyright file="ContactUsConfigurationSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class ContactUsConfigurationSetup
    {
        private readonly ScenarioContext context;
        private readonly ObjectContext objectContext;
        private readonly IConfigSection configSection;

        public ContactUsConfigurationSetup(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration set up cannot be initialised.");
            }

            this.configSection = this.context.Get<IConfigSection>();
            this.objectContext = this.context.Get<ObjectContext>();
        }

        [BeforeScenario(Order = 2)]
        public void SetUpProjectSpecificConfiguration()
        {
            var config = this.configSection.GetConfigSection<ContactUsConfiguration>();
            this.context.SetContactUsConfig(config);
            var mongoDbconfig = this.configSection.GetConfigSection<MongoDbConfig>();
            this.context.SetMongoDbConfig(mongoDbconfig);
            this.objectContext.Replace("browser", config.Browser);
            this.objectContext.Replace("build", config.BuildNumber);
            this.objectContext.Replace("EnvironmentName", config.EnvironmentName);
        }
    }
}
