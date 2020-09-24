using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.UnitTests.ModelValidationTests
{
    public static class ModelValidator
    {
        public static (bool isValid, List<ValidationResult> validationResults) TryValidateModel<TModel>(TModel model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            return (isValid, validationResults);
        }
    }
}
