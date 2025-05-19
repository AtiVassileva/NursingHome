using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models
{
    public class RegulatoryDocumentViewModel
    {
        [Required(ErrorMessage = "Моля въведете име на документа!")]
        [Display(Name = "Име на документа")]
        public string Title { get; set; } = null!;

        [Display(Name = "Документ")]
        public IFormFile File { get; set; } = null!;
    }
}