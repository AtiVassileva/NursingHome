using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models
{
    public class FeeBookViewModel
    {
        [Display(Name = "Месец")]
        public int SelectedMonth { get; set; }

        [Display(Name = "Година")]
        public int SelectedYear { get; set; }

        public List<FeeBookRowViewModel> Rows { get; set; } = new();
    }
}