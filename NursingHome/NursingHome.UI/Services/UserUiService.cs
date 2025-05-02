using Microsoft.AspNetCore.Identity;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Services
{
    using static Common.WebConstants;
    public class UserUiService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserUiService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<string> GetUserRoleNamesInBulgarian(string email)
        {
            IList<string> roles = new List<string>();

            Task.Run(async () =>
            {
                var user = await _userManager.FindByEmailAsync(email);
                roles = await _userManager.GetRolesAsync(user);

            })
                .GetAwaiter()
                .GetResult();

            var rolesInBulgarian = new List<string>();

            foreach (var role in roles.Distinct())
            {
                switch (role)
                {
                    case AdministratorRoleName:
                        rolesInBulgarian.Add("Администратор");
                        break;
                    case EmployeeRoleName:
                        rolesInBulgarian.Add("Служител");
                        break;
                    case RegularUserRoleName:
                        rolesInBulgarian.Add("Резидент");
                        break;
                }
            }

            return rolesInBulgarian;
        }
    }
}