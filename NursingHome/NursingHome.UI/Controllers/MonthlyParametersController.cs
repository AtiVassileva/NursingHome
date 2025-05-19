using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Controllers
{
    public class MonthlyParametersController : Controller
    {
        private readonly MonthlyParameterService _monthlyParameterService;

        public MonthlyParametersController(MonthlyParameterService monthlyParameterService)
        {
            _monthlyParameterService = monthlyParameterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _monthlyParameterService.GetMonthlyParametersByMonth(DateTime.Now.Month, DateTime.Now.Year)
                        ??
                        MonthlyParameter.CreateInstance(DateTime.Now.Year, DateTime.Now.Month);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMonthlyParameters(MonthlyParameter model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var isSaveSuccessful = await _monthlyParameterService.SaveMonthlyParameters(model);

            if (!isSaveSuccessful)
            {
                return BadRequest();
            }

            TempData["Success"] = "Месечните параметри са запазени успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}