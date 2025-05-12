using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class MedicalRecord : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }

        [Range(0, 100, ErrorMessage = "Процентът на ТЕЛК трябва да бъде в диапазона 0-100%!")]
        [Display(Name = "ТЕЛК Процент")]
        public int DisabilityPercent { get; set; }

        [Display(Name = "Алергии")]
        public string? Allergies { get; set; }

        [Display(Name = "Терапия")]
        public string? Therapy { get; set; }
    }
}