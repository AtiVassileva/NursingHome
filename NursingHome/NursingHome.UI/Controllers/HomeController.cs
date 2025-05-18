using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Models;
using System.Diagnostics;

namespace NursingHome.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return LocalRedirect("/Identity/Account/Login");
        }

        public IActionResult Contacts() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}