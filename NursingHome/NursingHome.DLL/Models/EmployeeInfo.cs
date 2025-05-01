using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;

    public class EmployeeInfo : BaseEntity
    {
        [Required] public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }

        [Required] public EmployeePosition EmployeePosition { get; set; }
    }
}