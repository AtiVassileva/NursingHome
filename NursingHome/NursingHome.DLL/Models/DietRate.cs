using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class DietRate : BaseEntity
    {
        [Required(ErrorMessage = "Моля изберете вид диета!")]
        public DietNumber DietNumber { get; set; }

        [Required(ErrorMessage = "Моля въведете месечна такса!")]
        [Display(Name = "Месечна такса")]
        public decimal MonthlyRate { get; set; }

        [Required]
        public Guid MonthId { get; set; }

        public MonthlyParameter? Month { get; set; }
    }
}