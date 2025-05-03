using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserUiService _userUiService;

        public AdminController(UserUiService userUiService)
        {
            _userUiService = userUiService;
        }

        public async Task<IActionResult> EmployeeAccounts()
        {
            var employees = await _userUiService.GetEmployees();
            return View(employees.ToList());
        }
        
        public async Task<IActionResult> ResidentsAccounts()
        {
            var residents = await _userUiService.GetResidents();
            return View(residents);
        }
    }
}