using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class WhyContactUsBodyViewModel
    {
        private const string SelectedCategoryValidationError = "Choose an category";
        private const string MoreDetailValidationError = "Enter a message describing the issue";

        public enum SelectCategory
        {
            None,
            AdviceGuidance,
            Courses,
            Website,
            Feedback,
            SomethingElse,
        }

        [Required(ErrorMessage = SelectedCategoryValidationError)]
        [Range((int)SelectCategory.AdviceGuidance, (int)SelectCategory.SomethingElse, ErrorMessage = SelectedCategoryValidationError)]
        public SelectCategory SelectedCategory { get; set; } = SelectCategory.None;

        [Display(Name = "Tell us more about why you want to contact us, in as much details as you can. Don't include any personal or account information.")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = MoreDetailValidationError)]
        [StringLength(1000, ErrorMessage = MoreDetailValidationError)]
        public string? MoreDetail { get; set; }
    }
}
