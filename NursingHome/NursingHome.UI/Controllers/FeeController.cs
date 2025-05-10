using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Models;
using NursingHome.UI.PdfGenerators;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class FeeController : Controller
    {
        private readonly UserUiService _userUiService;
        private readonly MonthlyParameterService _monthlyParameterService;
        private readonly MonthlyFeeService _monthlyFeeService;
        private readonly FeeBookUiService _feeBookUiService;
        private readonly IMapper _mapper;

        public FeeController(UserUiService userUiService, MonthlyParameterService monthlyParameterService, MonthlyFeeService monthlyFeeService, IMapper mapper, FeeBookUiService feeBookUiService)
        {
            _userUiService = userUiService;
            _monthlyParameterService = monthlyParameterService;
            _monthlyFeeService = monthlyFeeService;
            _mapper = mapper;
            _feeBookUiService = feeBookUiService;
        }

        [HttpGet]
        public async Task<IActionResult> Calculate(MonthlyFeeViewModel? passedModel = null)
        {
            var model = passedModel ?? new MonthlyFeeViewModel();

            if (passedModel != null)
            {
                await SetUpFeeViewModel(model);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetFee(string userId, int month, int year)
        {
            var fee = await _monthlyFeeService.GetMonthlyFeeForUserByMonth(userId, month, year);

            return Json(fee);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFeeBook()
        {
            var vm = await _feeBookUiService.CreateFeeBookViewModel();

            return View("FeeBook", vm);
        }

        public async Task<IActionResult> ExportFeeBookPdf()
        {
            var vm = await _feeBookUiService.CreateFeeBookViewModel();
            var pdfFile = FeeBookPdfGenerator.Generate(vm);

            return File(pdfFile, "application/pdf", $"ТаксоваКнига_{vm.SelectedMonth}_{vm.SelectedYear}.pdf");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(MonthlyFeeViewModel model, string? userId)
        {
            if (!ModelState.IsValid)
            {
                await SetUpFeeViewModel(model);

                return View(model);
            }

            var user = await _userUiService.GetById(model.SelectedUserId);

            if (user == null)
            {
                ModelState.AddModelError("", "Потребителят не е намерен!");
                return View(model);
            }

            var monthParams = await _monthlyParameterService.GetMonthlyParametersByMonth(model.Month, model.Year);

            if (monthParams == null)
            {
                ModelState.AddModelError("", "Няма зададени месечни параметри!");
                return View(model);
            }

            var feesTuple = await _monthlyFeeService.CalculateMonthlyFee(user, monthParams, model.PresentDays);

            model.RealCost = feesTuple.Item1;
            model.FeeAmount = feesTuple.Item2;

            var monthlyFeeModel = _mapper.Map<MonthlyFee>(model);
            var isSaveSuccessful = await _monthlyFeeService.SaveMonthlyFee(monthlyFeeModel);

            if (!isSaveSuccessful)
                return BadRequest();

            return RedirectToAction(nameof(Calculate), model);
        }

        private async Task SetUpFeeViewModel(MonthlyFeeViewModel model)
        {
            model.Month = DateTime.Now.Month;
            model.Year = DateTime.Now.Year;

            var users = await _userUiService.GetActiveResidents();

            var usersSelectList = users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.FirstName} {u.MiddleName} {u.LastName}"
                })
                .ToList();

            model.AllUsers = usersSelectList;
        }
    }
}