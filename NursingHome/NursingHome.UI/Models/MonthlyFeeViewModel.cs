using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models
{
    public class MonthlyFeeViewModel
    {
        [Required(ErrorMessage = "Моля, изберете потребител!")]
        [Display(Name = "Потребител")]
        public string SelectedUserId { get; set; } = null!;

        public List<SelectListItem> AllUsers { get; set; } = new();

        [Display(Name = "Месец")]
        [Required(ErrorMessage = "Mоля, изберете месец!")]
        public int Month { get; set; }

        [Display(Name = "Година")]
        [Required(ErrorMessage = "Моля, изберете месец!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Моля, въведете брой присъствени дни!")]
        [Display(Name = "Присъствени дни")]
        [Range(0, 31, ErrorMessage = "Моля въведете присъствени дни в интервала 0 - 31!")]
        public int PresentDays { get; set; }

        [Display(Name = "Реална издръжка")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal RealCost { get; set; }

        [Display(Name = "Дължима сума")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal FeeAmount { get; set; }
    }
}