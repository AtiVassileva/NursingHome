using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using  static Common.ModelConstants;

    public class Report : BaseEntity
    {
        public int? Month { get; set; }
        public int? Year { get; set; }

        [Required]
        public string FileName { get; set; } = null!; 

        [Required]
        public string FilePath { get; set; } = null!; 

        [Required]
        public string ContentType { get; set; } = null!;

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public ReportType Type { get; set; } 
        
        [Required]
        public string UploadedById { get; set; } = null!;
        
        public ApplicationUser? UploadedBy { get; set; } = null!;
    }
}