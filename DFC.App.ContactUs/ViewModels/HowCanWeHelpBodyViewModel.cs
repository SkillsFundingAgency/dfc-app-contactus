using DFC.App.ContactUs.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HowCanWeHelpBodyViewModel
    {
        public const string SelectedCategoryValidationError = "Choose a category";

        public const string MoreDetailRequiredError = "Enter a message describing the issue";

        public const string MoreDetailLengthValidationError = "Message is limited to between 1 and {1} characters";
        private const string RegExForMoreDetail = "^[\\w\\s!\"£$%^&*() _+=\\-\\[\\]\\}\\{;'#~@:,.\\/\\?]*$";
        private const string MoreDetailInvalidCharactersValidationError = "Message contains invalid characters";

        [Required(ErrorMessage = SelectedCategoryValidationError)]
        [Range((int)Category.AdviceGuidance, (int)Category.Other, ErrorMessage = SelectedCategoryValidationError)]
        [EnumDataType(typeof(Category))]
        public Category? SelectedCategory { get; set; }

        [Display(Name = "Tell us why you want to contact us in as much detail as you can.\nDon’t include any personal or account information.")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = MoreDetailRequiredError)]
        [RegularExpression(RegExForMoreDetail, ErrorMessage = MoreDetailInvalidCharactersValidationError)]
        [StringLength(1000, ErrorMessage = MoreDetailLengthValidationError)]
        public string? MoreDetail { get; set; }

        public bool IsCallback { get; set; }
    }
}
