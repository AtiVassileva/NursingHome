using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;

    public class WorkSchedule : BaseEntity
    {
        [Required]
        public EmployeePosition EmployeePosition { get; set; } 

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}