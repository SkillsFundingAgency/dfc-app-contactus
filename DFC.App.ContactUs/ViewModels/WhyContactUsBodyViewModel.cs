using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class WhyContactUsBodyViewModel
    {
        private const string SelectedCategoryValidationError = "Choose a category";
        private const string MoreDetailValidationError = "Enter a message describing the issue";

        public enum SelectCategory
        {
            [Description("None")]
            None,
            [Description("Careers advice and guidance")]
            AdviceGuidance,
            [Description("Courses, training and qualifications")]
            Courses,
            [Description("Problems using the website")]
            Website,
            [Description("Give feedback")]
            Feedback,
            [Description("Something else")]
            SomethingElse,
        }

        [Required(ErrorMessage = SelectedCategoryValidationError)]
        [Range((int)SelectCategory.AdviceGuidance, (int)SelectCategory.Website, ErrorMessage = SelectedCategoryValidationError)]
        [EnumDataType(typeof(SelectCategory))]
        public SelectCategory SelectedCategory { get; set; } = SelectCategory.None;

        [Display(Name = "Tell us more about why you want to contact us, in as much details as you can. Don't include any personal or account information.")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = MoreDetailValidationError)]
        [StringLength(1000, ErrorMessage = MoreDetailValidationError)]
        public string? MoreDetail { get; set; }
    }
}
