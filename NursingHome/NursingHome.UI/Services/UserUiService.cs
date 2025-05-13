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
        private readonly ResidentInfoService _residentInfoService;
        private readonly EmployeeInfoService _employeeInfoService;

        public UserUiService(UserManager<ApplicationUser> userManager, UserService userService, IMapper mapper, ResidentInfoService residentInfoService, EmployeeInfoService employeeInfoService)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
            _residentInfoService = residentInfoService;
            _employeeInfoService = employeeInfoService;
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
                return null!;
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

        public async Task<List<ApplicationUser>> GetActiveResidents()
        {
            var residents = await GetResidents();
            var activeResidents = residents
                .Where(r => r.UserStatus == UserStatus.Active)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.MiddleName)
                .ThenBy(u => u.LastName)
                .ToList();

            return activeResidents;
        }

        public async Task<List<ApplicationUser>> GetInactiveResidents()
        {
            var residents = await GetResidents();
            var inactiveResidents = residents
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.MiddleName)
                .ThenBy(u => u.LastName)
                .Where(r => r.UserStatus == UserStatus.Inactive)
                .ToList();

            return inactiveResidents;
        }

        public List<string> GetUserRoleNamesInBulgarian(string email)
        {
            IList<string> roles = new List<string>();

            Task.Run(async () =>
            {
                var user = await _userManager.FindByEmailAsync(email);
                roles = await _userManager.GetRolesAsync(user!);

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

        public async Task<List<ApplicationUser>> GetBirthdaysThisMonthAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var residents = await GetActiveResidents();

            var birthdays = residents
            .Where(u => u.ResidentInfo!.DateOfBirth.Month == currentMonth)
            .OrderBy(u => u.ResidentInfo!.DateOfBirth.Day)
            .ToList();

            return birthdays;
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

        public async Task<string> DeleteUser(string id)
        {
            var result = string.Empty;

            var user = await _userService.GetById(id);

            if (user == null)
                return string.Empty;

            var residentInfoForUser = await _residentInfoService.GetResidentInfoByUserId(id);

            if (residentInfoForUser != null)
            {
                await _residentInfoService.DeleteResidentInfo(residentInfoForUser.Id);
                result = "resident";
            }

            var employeeInfoForUser = await _employeeInfoService.GetEmployeeInfoByUserId(id);

            if (employeeInfoForUser != null)
            {
                await _employeeInfoService.DeleteEmployeeInfo(employeeInfoForUser.Id);
                result = "employee";
            }

            await _userManager.DeleteAsync(user);

            return result;
        }
    }
}