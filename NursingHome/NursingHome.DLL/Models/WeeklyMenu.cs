using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class WeeklyMenu : BaseEntity
    {
        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        [Required]
        public string ContentType { get; set; } = null!;

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime StartOfWeek { get; set; }

        [Required]
        public DateTime EndOfWeek { get; set; }

        [Required]
        public string UploadedById { get; set; } = null!;
        public ApplicationUser UploadedBy { get; set; } = null!;
    }
}