using Microsoft.AspNetCore.Identity;
using NursingHome.BLL;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Services
{
    using static Common.WebConstants;
    public class UserUiService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;

        public UserUiService(UserManager<ApplicationUser> userManager, UserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IList<ApplicationUser>> GetEmployees()
        {
            return await _userManager.GetUsersInRoleAsync(EmployeeRoleName);
        }
        
        public async Task<List<ApplicationUser>> GetResidents()
        {
            var residents = await _userManager.GetUsersInRoleAsync(RegularUserRoleName);
            var residentsWithInfo = await _userService.GetUsersWithResidentInfo(residents);

            return residentsWithInfo;
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