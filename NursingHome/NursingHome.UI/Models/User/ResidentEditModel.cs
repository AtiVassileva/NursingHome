using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Models.User
{
    public class ResidentEditModel : BaseEditModel
    {
        [Required(ErrorMessage = "Моля въведете дата на постъпване!")]
        [Display(Name = "Дата на постъпване")]
        public DateTime? DateAdmitted { get; set; }

        [Display(Name = "Дата на изписване")]
        public DateTime? DateDischarged { get; set; }

        [Required(ErrorMessage = "Моля въведете дата на раждане!")]
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

        [Required(ErrorMessage = "Моля въведете име на личен лекар!")]
        [Display(Name = "Личен лекар")]
        public string? GpName { get; set; }

        [Required(ErrorMessage = "Моля въведете местоположение на личен лекар!")]
        [Display(Name = "Местоположение на лекаря")]
        public string? GpLocation { get; set; }

        [Required(ErrorMessage = "Моля въведете телефон на личен лекар!")]
        [Display(Name = "Телефон на личния лекар")]
        public string? GpPhoneNumber { get; set; }

        [Required(ErrorMessage = "Моля въведете име на роднина!")]
        [Display(Name = "Име на близък роднина")]
        public string? FamilyMemberName { get; set; }

        [Required(ErrorMessage = "Моля въведете телефон на роднина!")]
        [Display(Name = "Телефон на роднина")]
        public string? FamilyMemberPhoneNumber { get; set; }

        [Display(Name = "Пенсия")]
        [Range(0, int.MaxValue, ErrorMessage = "Пенсията не може да бъде отрицателно число!")]
        public decimal? Pension { get; set; }

        [Display(Name = "Наем" )]
        [Range(0, int.MaxValue, ErrorMessage = "Наемът не може да бъде отрицателно число!")]
        public decimal? Rent { get; set; }

        [Display(Name = "Заплата")]
        [Range(0, int.MaxValue, ErrorMessage = "Заплатата не може да бъде отрицателно число!")]
        public decimal? Salary { get; set; }

        [Display(Name = "Други доходи")]
        [Range(0, int.MaxValue, ErrorMessage = "Другите доходи не могат да бъдат отрицателно число!")]
        public decimal? OtherIncome { get; set; }

        [Display(Name = "Има наследство")]
        public bool HasInheritance { get; set; }

        [Display(Name = "Има договор за издръжка")]
        public bool HasSupportContract { get; set; }

        [Display(Name = "Има продажба на недвижим имот")]
        public bool HasRealEstateSale { get; set; }

        [Display(Name = "Има дарение")]
        public bool HasDonation { get; set; }

        public List<SelectListItem> AvailableEmployees { get; set; } = new();
    }
}