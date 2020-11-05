using DFC.App.ContactUs.Data.Common;
using DFC.App.ContactUs.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Xunit;

namespace DFC.App.ContactUs.Data.UnitTests.ValidationTests
{
    [Trait("Category", "EmailModel Validation Unit Tests")]
    public class EmailModelValidationTests
    {
        private const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        [Theory]
        [InlineData(null)]
        [InlineData(GuidEmpty)]
        public void CanCheckIfDocumentIdIsInvalid(Guid documentId)
        {
            // Arrange
            var model = CreateModel(documentId, "A title", "<p>some content</p>");

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 1);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, ValidationMessage.FieldInvalidGuid, nameof(model.Id)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))).ErrorMessage);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("0123456789")]
        [InlineData("abc")]
        [InlineData("xyz123")]
        [InlineData("abc_def")]
        [InlineData("abc-def")]
        public void CanCheckITitleIsValid(string title)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), title, "<p>some content</p>");

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        private EmailModel CreateModel(Guid documentId, string title, string body)
        {
            var model = new EmailModel
            {
                Id = documentId,
                Title = title,
                Url = new Uri("aaa-bbb", UriKind.Relative),
                Body = body,
                LastReviewed = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                LastCached = DateTime.UtcNow,
            };

            return model;
        }

        private List<ValidationResult> Validate(EmailModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
