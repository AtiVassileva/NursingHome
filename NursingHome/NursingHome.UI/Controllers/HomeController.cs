using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Models;
using System.Diagnostics;
using NursingHome.BLL;

namespace NursingHome.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService _userService;

        public HomeController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> UserAccounts()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
