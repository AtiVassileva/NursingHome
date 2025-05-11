using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models
{
    public class PaymentViewModel
    {
        [Display(Name = "Месец")]
        public int SelectedMonth { get; set; }

        [Display(Name = "Година")]
        public int SelectedYear { get; set; }

        public List<PaymentRowViewModel> Payments { get; set; } = new();
    }
}