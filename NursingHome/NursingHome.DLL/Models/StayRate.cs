using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class StayRate : BaseEntity
    {
        [Required(ErrorMessage = "Моля изберете вид стая!")]
        public RoomType RoomType { get; set; }

        [Required(ErrorMessage = "Моля въведете месечна такса!")]
        [Display(Name = "Месечна такса")]
        public decimal MonthlyRate { get; set; }

        [Required]
        public Guid MonthId { get; set; }

        public MonthlyParameter? Month { get; set; }
    }
}