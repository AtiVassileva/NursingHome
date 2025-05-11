using System.ComponentModel.DataAnnotations;
using NursingHome.DAL.Common;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models
{
    public class PaymentRowViewModel
    {
        public Guid PaymentId { get; set; }

        [Display(Name = "Три имена")]
        public string ResidentName { get; set; } = null!;

        [Display(Name = "Дължима сума")]
        public decimal FeeAmount { get; set; }

        [Display(Name = "Статус")] 
        public PaymentStatus Status { get; set; }
    }
}