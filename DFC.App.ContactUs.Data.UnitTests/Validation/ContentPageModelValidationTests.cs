using DFC.App.ContactUs.Data.Common;
using DFC.App.ContactUs.Data.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.App.ContactUs.Data.UnitTests.Validation
{
    [TestFixture]
    [Category("AppValidation.Tests")]
    public class ContentPageModelValidationTests
    {
        private const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(null)]
        [TestCase(GuidEmpty)]
        public void CanCheckIfDocumentIdIsInvalid(Guid documentId)
        {
            var model = CreateModel(documentId, "canonicalname1", "content1", new List<string>());

            var vr = Validate(model);

            vr.Should().NotBeEmpty();
            vr.Should().Contain(x => x.ErrorMessage == string.Format(CultureInfo.InvariantCulture, ValidationMessage.FieldInvalidGuid, nameof(model.DocumentId)));
            vr.Should().HaveCount(1);
        }

        [TestCase("abcdefghijklmnopqrstuvwxyz")]
        [TestCase("0123456789")]
        [TestCase("abc")]
        [TestCase("xyz123")]
        [TestCase("abc_def")]
        [TestCase("abc-def")]
        public void CanCheckIfCanonicalNameIsValid(string canonicalName)
        {
            var model = CreateModel(Guid.NewGuid(), canonicalName, "content", new List<string>());

            var vr = Validate(model);

            vr.Should().BeEmpty();
        }

        [TestCase("ABCDEF")]
        public void CanCheckIfCanonicalNameIsInvalid(string canonicalName)
        {
            var model = CreateModel(Guid.NewGuid(), canonicalName, "content", new List<string>());

            var vr = Validate(model);

            vr.Should().NotBeEmpty();
            vr.Should().Contain(x => x.ErrorMessage == string.Format(CultureInfo.InvariantCulture, ValidationMessage.FieldNotLowercase, nameof(model.CanonicalName)));
            vr.Should().HaveCount(1);
        }

        [TestCase("abcdefghijklmnopqrstuvwxyz")]
        [TestCase("0123456789")]
        [TestCase("abc")]
        [TestCase("xyz123")]
        [TestCase("abc_def")]
        [TestCase("abc-def")]
        public void CanCheckIfAlternativeNameIsValid(string alternativeName)
        {
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", new List<string>() { alternativeName });

            var vr = Validate(model);

            vr.Should().BeEmpty();
        }

        [TestCase("ABCDEF")]
        public void CanCheckIfAlternativeNameIsInvalid(string alternativeName)
        {
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", new List<string>() { alternativeName });

            var vr = Validate(model);

            vr.Should().NotBeEmpty();
            vr.Should().Contain(x => x.ErrorMessage == string.Format(CultureInfo.InvariantCulture, ValidationMessage.FieldNotLowercase, nameof(model.AlternativeNames)));
            vr.Should().HaveCount(1);
        }

        private ContentPageModel CreateModel(Guid documentId, string canonicalName, string content, List<string> alternativeNames)
        {
            var model = new ContentPageModel
            {
                DocumentId = documentId,
                CanonicalName = canonicalName,
                BreadcrumbTitle = canonicalName,
                Version = Guid.NewGuid(),
                Url = "https://localhost",
                Content = content,
                AlternativeNames = alternativeNames.ToArray(),
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        private List<ValidationResult> Validate(ContentPageModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
