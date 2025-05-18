using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class WeeklyMenusController : Controller
    {
        private readonly FileUiService _fileUiService;
        private readonly WeeklyMenuService _weeklyMenuService;

        public WeeklyMenusController(FileUiService fileUiService, WeeklyMenuService weeklyMenuService)
        {
            _fileUiService = fileUiService;
            _weeklyMenuService = weeklyMenuService;
        }

        public async Task<IActionResult> Index()
        {
            var dt = DateTime.Today;
            var currentMenu = await _weeklyMenuService.GetWeeklyMenu(dt.GetMondayOfCurrentWeek(), dt.GetSundayOfCurrentWeek());

            return View(currentMenu);
        }

        public IActionResult Upload() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Моля, изберете файл!");
                return View();
            }

            var isUploadSuccessful = await _fileUiService.UploadWeeklyMenu(file, User);

            if (!isUploadSuccessful)
            {
                return View(file);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}