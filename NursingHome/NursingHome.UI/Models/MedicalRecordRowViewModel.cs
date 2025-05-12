using NursingHome.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models
{
    public class MedicalRecordRowViewModel
    {
        public string UserId { get; set; } = null!;

        [Display(Name ="Три имена")]
        public string FullName { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Процентът на ТЕЛК трябва да бъде в диапазона 0-100%!")]
        [Display(Name = "ТЕЛК Процент")]
        public int DisabilityPercent { get; set; }

        [Display(Name = "Алергии")]
        public string? Allergies { get; set; }

        [Display(Name = "Терапия")]
        public string? Therapy { get; set; }

        [Display(Name = "Диета")]
        public string? Diet { get; set; }
    }
}