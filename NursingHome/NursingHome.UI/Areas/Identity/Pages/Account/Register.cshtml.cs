using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NursingHome.DAL.Models;
using NursingHome.UI.Models.User;
using static NursingHome.DAL.Common.ModelConstants;
using static NursingHome.UI.Common.WebConstants;

namespace NursingHome.UI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty] public UserRegisterModel Input { get; set; } = new();
        public List<SelectListItem>? Roles { get; set; }

        public async Task OnGetAsync()
        {
            Input = new UserRegisterModel();
            Roles = await _roleManager.Roles
                .Where(r => r.Name != AdministratorRoleName)
                .Select(r => new SelectListItem {Text = r.Name, Value = r.Id})
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}