using System.ComponentModel.DataAnnotations;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models.User
{
    public class BaseEditModel
    {
        public string Id { get; set; } = null!;
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
        
        [Display(Name = "Статус на потребителя")]
        public UserStatus UserStatus { get; set; }
    }
}