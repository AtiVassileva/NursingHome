using NursingHome.DAL.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models
{
    public class SocialDocumentViewModel
    {
        public Guid Id { get; set; }
        public string ResidentName { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public SocialDocumentType DocumentType { get; set; }
        public DateTime UploadedOn { get; set; }
        public ApplicationUser UploadedBy { get; set; }
        public string UploadedById { get; set; } = null!;
    }
}