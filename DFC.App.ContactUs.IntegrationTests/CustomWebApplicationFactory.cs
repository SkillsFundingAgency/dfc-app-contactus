﻿using AutoMapper;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.IntegrationTests.Extensions;
using DFC.App.ContactUs.IntegrationTests.Fakes;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DFC.App.ContactUs.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
            this.MockCosmosRepo = A.Fake<ICosmosRepository<ConfigurationSetModel>>();
            this.MockSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
        }

        internal ICosmosRepository<ConfigurationSetModel> MockCosmosRepo { get; set; }

        internal ISessionStateService<SessionDataModel> MockSessionStateService { get; set; }

        public HttpClient CreateClientWithWebHostBuilder()
        {
            return this.WithWebHostBuilder(builder =>
            {
                builder.RegisterServices(this.MockCosmosRepo,this.MockSessionStateService);
            }).CreateClient();
        }

        internal IEnumerable<ConfigurationSetModel> GetContentPageModels()
        {
            return new List<ConfigurationSetModel>
            {
                new ConfigurationSetModel
                {
                    Id = Guid.NewGuid(),
                    Url = new Uri("http://www.test.com"),
                },
                new ConfigurationSetModel
                {
                    Id = Guid.NewGuid(),
                    Url = new Uri("http://www.test.com"),
                },
            };
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder?.ConfigureServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                services.AddSingleton<IConfiguration>(configuration);
            });
        }
    }
}
