using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models.User
{
    public class ResidentRegisterModel
    {
        [Display(Name = "Дата на постъпване")]
        public DateTime? DateAdmitted { get; set; }

        [Display(Name = "Дата на изписване")]
        public DateTime? DateDischarged { get; set; }
        
        [Display(Name = "Дата на раждане")]
        public DateTime? DateOfBirth { get; set; } 

        [Display(Name = "Телефонен номер")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Пол")]
        public Gender Gender { get; set; }
        
        [Display(Name = "Диета")]
        public DietNumber DietNumber { get; set; }
        
        [Display(Name = "Тип стая")]
        public RoomType RoomType { get; set; }
        
        [Display(Name = "Отговорен служител")]
        public string? EmployeeManagerId { get; set; } 

        [Display(Name = "Личен лекар")]
        public string? GpName { get; set; } 
        
        [Display(Name = "Местоположение на лекаря")]
        public string? GpLocation { get; set; } 
        
        [Display(Name = "Телефон на личния лекар")]
        public string? GpPhoneNumber { get; set; } 
        
        [Display(Name = "Име на близък роднина")]
        public string? FamilyMemberName { get; set; } 
        
        [Display(Name = "Телефон на роднина")]
        public string? FamilyMemberPhoneNumber { get; set; } 

        [Display(Name = "Пенсия")]
        [Range(0, int.MaxValue)]
        public decimal? Pension { get; set; }

        [Display(Name = "Наем")]
        [Range(0, int.MaxValue)]
        public decimal? Rent { get; set; }

        [Display(Name = "Заплата")]
        [Range(0, int.MaxValue)]
        public decimal? Salary { get; set; }

        [Display(Name = "Други доходи")]
        [Range(0, int.MaxValue)]
        public decimal? OtherIncome { get; set; }

        [Display(Name = "Има наследство")]
        public bool HasInheritance { get; set; }

        [Display(Name = "Има договор за издръжка")]
        public bool HasSupportContract { get; set; }

        [Display(Name = "Има продажба на недвижим имот")]
        public bool HasRealEstateSale { get; set; }

        [Display(Name = "Има дарение")]
        public bool HasDonation { get; set; }
    }
}