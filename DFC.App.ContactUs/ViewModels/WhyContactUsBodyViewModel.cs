using DFC.App.ContactUs.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class WhyContactUsBodyViewModel
    {
        public const string SelectedCategoryValidationError = "Choose a category";

        public const string MoreDetailRequiredError = "Enter a message describing the issue";

        public const string MoreDetailInvalidValidationError = "More details is too long or contains invalid characters";

        [Required(ErrorMessage = SelectedCategoryValidationError)]
        [Range((int)Category.AdviceGuidance, (int)Category.SomethingElse, ErrorMessage = SelectedCategoryValidationError)]
        [EnumDataType(typeof(Category))]
        public Category SelectedCategory { get; set; } = Category.None;

        [Display(Name = "Tell us more about why you want to contact us, in as much details as you can. Don't include any personal or account information.")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = MoreDetailRequiredError)]
        [StringLength(1000, ErrorMessage = MoreDetailInvalidValidationError)]
        public string? MoreDetail { get; set; }
    }
}
