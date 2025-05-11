using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;
    public class Payment : BaseEntity
    {
        [Required]
        [Display(Name = "Статус")]
        public PaymentStatus Status { get; set; }
        [Required] public Guid MonthlyFeeId { get; set; }
        public MonthlyFee? MonthlyFee { get; set; }
    }
}