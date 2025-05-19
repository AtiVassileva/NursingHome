using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models
{
    public class UploadSocialDocumentViewModel
    {
        [Required(ErrorMessage = "Моля, изберете потребител!")] 
        public string ResidentId { get; set; } = null!;

        [Required]
        public SocialDocumentType DocumentType { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;

        public List<SelectListItem> Residents { get; set; } = new();
    }
}