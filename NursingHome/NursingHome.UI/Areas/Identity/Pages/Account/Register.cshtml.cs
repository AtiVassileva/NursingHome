using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NursingHome.BLL;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Models.User;
using static NursingHome.DAL.Common.ModelConstants;
using static NursingHome.UI.Common.WebConstants;

namespace NursingHome.UI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            UserService userService, 
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty] public UserRegisterModel Input { get; set; } = new();
        public List<SelectListItem>? Roles { get; set; } = new();
        public List<SelectListItem> AvailableEmployees { get; set; } = new();

        public async Task OnGetAsync()
        {
            Input = new UserRegisterModel();

            Roles = await _roleManager.Roles
                .Where(r => r.Name != AdministratorRoleName)
                .Select(r => new SelectListItem {
                    Value = r.Id, 
                    Text = r.Name == "Employee" ? "Служител" : "Потребител"
                })
                .ToListAsync();

            var employees = await _userManager.GetUsersInRoleAsync(EmployeeRoleName);
            var usersWithEmployeeInfo = await _userService.GetUsersWithEmployeeInfo(employees);

            AvailableEmployees = usersWithEmployeeInfo
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.FirstName} {u.LastName} - {u.EmployeeInfo?.EmployeePosition.GetDisplayName()}"
                })
                .ToList();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var selectedRole = await _roleManager.FindByIdAsync(Input.UserRoleId);
            if (selectedRole == null)
            {
                ModelState.AddModelError(string.Empty, "Невалидна роля!");
                return Page();
            }

            Input.SelectedRoleName = selectedRole.Name;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    MiddleName = Input.MiddleName,
                    LastName = Input.LastName,
                    UserStatus = UserStatus.Active
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    switch (selectedRole.Name)
                    {
                        case EmployeeRoleName:
                        {
                            await _userManager.AddToRoleAsync(user, EmployeeRoleName);
                            var employeeInfo = _mapper.Map<EmployeeInfo>(Input.EmployeeInfo);
                            await _userService.RecordEmployeeInfo(user.Id, employeeInfo);
                            break;
                        }
                        case RegularUserRoleName:
                        {
                            await _userManager.AddToRoleAsync(user, RegularUserRoleName);
                            var residentInfo = _mapper.Map<ResidentInfo>(Input.ResidentInfo);
                            await _userService.RecordResidentInfo(user.Id, residentInfo);
                            break;
                        }
                    }

                    TempData["ShowSuccessToast"] = "true";
                    TempData["SuccessMessage"] = "Успешна регистрация!";

                    return RedirectToAction("ResidentsAccounts", "Admin");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return RedirectToAction("Index", "Home");
            }

            return BadRequest();
        }
    }
}