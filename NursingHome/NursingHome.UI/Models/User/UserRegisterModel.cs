using Microsoft.AspNetCore.Mvc.Rendering;
using NursingHome.DAL.Common;
using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models.User
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Моля, въведете име!")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете презиме!")]
        [Display(Name = "Презиме")]
        public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете фамилия!")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете имейл!")]
        [EmailAddress]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете парола!")]
        [StringLength(100, ErrorMessage = "Паролата трябва да е поне {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Моля, потвърдете паролата!")]
        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат!")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Моля, изберете статус!")]
        [Display(Name = "Статус на потребителя")]
        public UserStatus UserStatus { get; set; }

        [Required] 
        public string UserRoleId { get; set; } = null!;

        public List<SelectListItem> UserStatusOptions { get; } = new()
        {
            new SelectListItem { Value = ((int)UserStatus.Active).ToString(), Text = "Активен" },
            new SelectListItem { Value = ((int)UserStatus.Inactive).ToString(), Text = "Неактивен" },
        };
    }
}
