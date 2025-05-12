using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; } = null!;
        [Required] public string MiddleName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        
        public ResidentInfo? ResidentInfo { get; set; }
        public EmployeeInfo? EmployeeInfo { get; set; }

        [Required] public UserStatus UserStatus { get; set; }

        public MedicalRecord? MedicalRecord { get; set; }
    }
}