using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Models.User;

namespace NursingHome.UI.Services
{
    using static Common.WebConstants;
    using static NursingHome.DAL.Common.ModelConstants;

    public class UserUiService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserUiService(UserManager<ApplicationUser> userManager, UserService userService, IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IList<ApplicationUser>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync(EmployeeRoleName);
            var employeesWithInfo = await _userService.GetUsersWithEmployeeInfo(employees);

            return employeesWithInfo;
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

        public async Task<IdentityResult> EditResident(string id, ResidentEditModel model)
        {
            var user = await _userManager.Users
                .Include(u => u.ResidentInfo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return IdentityResult.Failed();

            model.UserStatus = model.DateDischarged is not null ? UserStatus.Inactive : UserStatus.Active;

            _mapper.Map(model, user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return IdentityResult.Success;

            return IdentityResult.Failed();
        }
        
        public async Task<IdentityResult> EditEmployee(string id, EmployeeEditModel model)
        {
            var user = await _userManager.Users
                .Include(u => u.EmployeeInfo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return IdentityResult.Failed();

            model.UserStatus = UserStatus.Active;
            _mapper.Map(model, user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return IdentityResult.Success;

            return IdentityResult.Failed();
        }
    }
}