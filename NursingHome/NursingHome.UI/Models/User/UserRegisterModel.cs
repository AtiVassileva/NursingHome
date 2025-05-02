using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models.User
{
    using static Common.WebConstants;
    public class UserRegisterModel : IValidatableObject
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
        public string? SelectedRoleName { get; set; }

        public ResidentRegisterModel? ResidentInfo { get; set; } = null;
        public EmployeeRegisterModel? EmployeeInfo { get; set; } = null;
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedRoleName == RegularUserRoleName)
            {
                if (ResidentInfo == null)
                {
                    yield return new ValidationResult("Моля попълнете информацията за потребител!", new[] { nameof(ResidentInfo) });
                }
                else
                {
                    if (ResidentInfo.DateAdmitted == null || ResidentInfo.DateAdmitted == default)
                        yield return new ValidationResult("Моля, въведете дата на постъпване!", new[] { "ResidentInfo.DateAdmitted" });

                    if (ResidentInfo.DateAdmitted == null || ResidentInfo.DateOfBirth == default)
                        yield return new ValidationResult("Моля, въведете дата на раждане!", new[] { "ResidentInfo.DateOfBirth" });

                    if (string.IsNullOrWhiteSpace(ResidentInfo.GpName))
                        yield return new ValidationResult("Моля, въведете име на личния лекар!", new[] { "ResidentInfo.GpName" });

                    if (string.IsNullOrWhiteSpace(ResidentInfo.GpLocation))
                        yield return new ValidationResult("Моля, въведете местоположение на личния лекар!", new[] { "ResidentInfo.GpLocation" });

                    if (string.IsNullOrWhiteSpace(ResidentInfo.GpPhoneNumber))
                        yield return new ValidationResult("Моля, въведете телефонен номер на личния лекар!", new[] { "ResidentInfo.GpPhoneNumber" });

                    if (string.IsNullOrWhiteSpace(ResidentInfo.FamilyMemberName))
                        yield return new ValidationResult("Моля, въведете име на роднина!", new[] { "ResidentInfo.FamilyMemberName" });

                    if (string.IsNullOrWhiteSpace(ResidentInfo.FamilyMemberPhoneNumber))
                        yield return new ValidationResult("Моля, въведете телефонен номер на роднина!", new[] { "ResidentInfo.FamilyMemberPhoneNumber" });

                }
            }
            else if (SelectedRoleName == EmployeeRoleName)
            {
                if (EmployeeInfo == null)
                {
                    yield return new ValidationResult("Моля попълнете информацията за служител!", new[] { nameof(EmployeeInfo) });
                }
                else
                {
                    if (!Enum.IsDefined(typeof(EmployeePosition), EmployeeInfo.EmployeePosition))
                        yield return new ValidationResult("Моля, изберете позиция!", new[] { "EmployeeInfo.EmployeePosition" });
                }
            }
        }
    }
}