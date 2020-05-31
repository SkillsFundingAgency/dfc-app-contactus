using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DFC.App.ContactUs.Attributes
{
    public class CustomValidationHtmlAttributeProvider : DefaultValidationHtmlAttributeProvider
    {
        private readonly IModelMetadataProvider metadataProvider;

        public CustomValidationHtmlAttributeProvider(IOptions<MvcViewOptions> optionsAccessor, IModelMetadataProvider metadataProvider, ClientValidatorCache clientValidatorCache)
            : base(optionsAccessor, metadataProvider, clientValidatorCache)
        {
            this.metadataProvider = metadataProvider;
        }

        public override void AddValidationAttributes(ViewContext viewContext, ModelExplorer modelExplorer, IDictionary<string, string> attributes)
        {
            _ = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
            _ = modelExplorer ?? throw new ArgumentNullException(nameof(modelExplorer));
            _ = attributes ?? throw new ArgumentNullException(nameof(attributes));

            base.AddValidationAttributes(viewContext, modelExplorer, attributes);

            var validationAttributeAdapterProvider = viewContext.HttpContext.RequestServices.GetRequiredService<IValidationAttributeAdapterProvider>();
            var context = new ClientModelValidationContext(viewContext, modelExplorer.Metadata, metadataProvider, attributes);

            if (modelExplorer.Container.Model is EnterYourDetailsBodyViewModel enterYourDetailsBodyViewModel)
            {
                AddValidationsForViewModel(enterYourDetailsBodyViewModel, modelExplorer, validationAttributeAdapterProvider, context);
            }
        }

        private static void AddValidationsForViewModel(EnterYourDetailsBodyViewModel viewModel, ModelExplorer modelExplorer, IValidationAttributeAdapterProvider validationAttributeAdapterProvider, ClientModelValidationContext context)
        {
            var properties = new Dictionary<string, string>()
                {
                    { nameof(EnterYourDetailsBodyViewModel.EmailAddress), nameof(EnterYourDetailsBodyViewModel.EmailAddressIsRequired) },
                    { nameof(EnterYourDetailsBodyViewModel.TelephoneNumber), nameof(EnterYourDetailsBodyViewModel.TelephoneNumberIsRequired) },
                    { nameof(EnterYourDetailsBodyViewModel.CallbackDateTime), nameof(EnterYourDetailsBodyViewModel.CallbackDateTimeIsRequired) },
                };
            var errorMessages = new Dictionary<string, string>()
                {
                    { nameof(EnterYourDetailsBodyViewModel.EmailAddress), "Enter your email address" },
                    { nameof(EnterYourDetailsBodyViewModel.TelephoneNumber), "Enter your telephone number" },
                    { nameof(EnterYourDetailsBodyViewModel.CallbackDateTime), "Enter when you want us to contact you" },
                };

            if (properties.Keys.Contains(modelExplorer.Metadata.PropertyName))
            {
                var propertyInfo = viewModel.GetType().GetProperty(properties[modelExplorer.Metadata.PropertyName]);

                if (propertyInfo != null)
                {
                    var isRequiredValue = (bool?)propertyInfo.GetValue(viewModel, null);

                    if (isRequiredValue != null && isRequiredValue.Value)
                    {
                        var validationAdapter = (RequiredAttributeAdapter)validationAttributeAdapterProvider.GetAttributeAdapter(new RequiredAttribute(), null);
                        var errorMessage = errorMessages[modelExplorer.Metadata.PropertyName];
                        validationAdapter.Attribute.ErrorMessage = errorMessage;
                        validationAdapter.AddValidation(context);
                    }
                }
            }
        }
    }
}
