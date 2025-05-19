using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class RegulatoryDocument : BaseEntity
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        [Required]
        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public string UploadedById { get; set; } = null!;
        
        public ApplicationUser? UploadedBy { get; set; } 
    }
}