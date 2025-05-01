using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;
    public class ResidentInfo : BaseEntity
    {
        [Required] public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        public DateTime DateAdmitted { get; set; }
        public DateTime? DateDischarged { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        [Required] public Gender Gender { get; set; }
        [Required] public DietNumber DietNumber { get; set; }
        [Required] public RoomType RoomType { get; set; }

        // Employee Manager Information
        [Required] public string EmployeeManagerId { get; set; } = null!;
        public ApplicationUser? EmployeeManager { get; set; }

        // GP Information
        [Required] public string GpName { get; set; } = null!;
        [Required] public string GpLocation { get; set; } = null!;
        [Required] public string GpPhoneNumber { get; set; } = null!;

        // Family Member Information
        [Required] public string FamilyMemberName { get; set; } = null!;
        [Required] public string FamilyMemberPhoneNumber { get; set; } = null!;

        // Incomes Information
        public decimal? Pension { get; set; }
        public decimal? Rent { get; set; }
        public decimal? Salary { get; set; }
        public decimal? OtherIncome { get; set; }

        // Fee Exceptions Information
        public bool HasInheritance { get; set; }
        public bool HasSupportContract { get; set; }
        public bool HasRealEstateSale { get; set; }
        public bool HasDonation { get; set; }
    }
}