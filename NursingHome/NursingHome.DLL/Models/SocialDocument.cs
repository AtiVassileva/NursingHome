using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class SocialDocument : BaseEntity
    {
        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        [Required]
        public string ContentType { get; set; } = null!;

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public SocialDocumentType DocumentType { get; set; }

        [Required]
        public string ResidentId { get; set; } = null!;

        public ApplicationUser Resident { get; set; } = null!;

        [Required]
        public string UploadedById { get; set; } = null!;

        public ApplicationUser UploadedBy { get; set; } = null!;
    }
}