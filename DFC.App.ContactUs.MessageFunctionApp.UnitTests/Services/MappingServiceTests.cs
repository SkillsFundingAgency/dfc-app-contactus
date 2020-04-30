using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Data.ServiceBusModels;
using DFC.App.ContactUs.MessageFunctionApp.AutoMapperProfile;
using DFC.App.ContactUs.MessageFunctionApp.Services;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.ContactUs.MessageFunctionApp.UnitTests.Services
{
    [Trait("Messaging Function", "Mapping Service Tests")]
    public class MappingServiceTests
    {
        private const int SequenceNumber = 123;
        private const string TestPageName = "Test Job name";
        private const string Title = "Title 1";
        private const string BreadcrumbTitle = "BreadcrumbTitle 1";
        private const bool IncludeInSitemap = true;
        private const string Description = "A description";
        private const string Keywords = "Some keywords";
        private const string Content = "<p>This is some content</p>";
        private static readonly IList<string> AlternativeNames = new string[] { "alt-name-1", "alt-name-2" };
        private static readonly DateTime LastModified = DateTime.UtcNow.AddDays(-1);
        private static readonly Guid ContentPageId = Guid.NewGuid();

        private readonly IMappingService mappingService;

        public MappingServiceTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ContentPageProfile());
            });

            var mapper = new Mapper(config);

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void MapToContentPageModelWhenContentPageMessageSentThenItIsMappedCorrectly()
        {
            var fullJPMessage = BuildContentPageMessage();
            var message = JsonConvert.SerializeObject(fullJPMessage);
            var expectedResponse = BuildExpectedResponse();

            // Act
            var actualMappedModel = mappingService.MapToContentPageModel(message, SequenceNumber);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualMappedModel);
        }

        private static ContentPageMessage BuildContentPageMessage()
        {
            return new ContentPageMessage
            {
                ContentPageId = ContentPageId,
                CanonicalName = TestPageName,
                LastModified = LastModified,
                Title = Title,
                AlternativeNames = AlternativeNames,
                IncludeInSitemap = IncludeInSitemap,
                Description = Description,
                Keywords = Keywords,
                Content = Content,
                BreadcrumbTitle = BreadcrumbTitle,
            };
        }

        private static ContentPageModel BuildExpectedResponse()
        {
            return new ContentPageModel
            {
                CanonicalName = TestPageName,
                DocumentId = ContentPageId,
                SequenceNumber = SequenceNumber,
                Etag = null,
                BreadcrumbTitle = BreadcrumbTitle,
                IncludeInSitemap = IncludeInSitemap,
                AlternativeNames = AlternativeNames,
                Content = Content,
                LastReviewed = LastModified,
                MetaTags = new MetaTagsModel()
                {
                    Title = Title,
                    Description = Description,
                    Keywords = Keywords,
                },
            };
        }
    }
}
