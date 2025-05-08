using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class MonthlyFee : BaseEntity
    {
        [Required] 
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "Моля, изберете месец!")] 
        public int Month { get; set; }

        [Required(ErrorMessage = "Моля, изберете месец!")] 
        public int Year { get; set; }

        [Required(ErrorMessage = "Моля, въведете брой присъствени дни!")]
        [Display(Name = "Брой присъствени дни")]
        public int PresentDays { get; set; }

        [Required(ErrorMessage = "Моля, въведете реална издръжка!")]
        [Display(Name = "Реална издръжка")]
        public decimal RealCost { get; set; }

        [Required(ErrorMessage = "Моля, въведете дължима сума!")]
        [Display(Name = "Дължима сума")]
        public decimal FeeAmount { get; set; }
    }
}