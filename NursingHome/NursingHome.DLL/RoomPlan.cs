using NursingHome.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL
{
    public class RoomPlan : BaseEntity
    {
        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public string UploadedById { get; set; } = null!;

        public ApplicationUser UploadedBy { get; set; } = null!;
    }
}