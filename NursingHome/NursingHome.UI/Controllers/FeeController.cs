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
        private readonly UserService _userService;
        private readonly UserUiService _userUiService;
        private readonly MonthlyParameterService _monthlyParameterService;
        private readonly MonthlyFeeService _monthlyFeeService;
        private readonly IMapper _mapper;

        public FeeController(UserUiService userUiService, MonthlyParameterService monthlyParameterService, UserService userService, MonthlyFeeService monthlyFeeService, IMapper mapper)
        {
            _userUiService = userUiService;
            _monthlyParameterService = monthlyParameterService;
            _userService = userService;
            _monthlyFeeService = monthlyFeeService;
            _mapper = mapper;
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
            var vm = await CreateFeeBookViewModel();

            return View("FeeBook", vm);
        }

        public async Task<IActionResult> ExportFeeBookPdf()
        {
            var vm = await CreateFeeBookViewModel();
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

        private async Task<FeeBookViewModel> CreateFeeBookViewModel()
        {
            var users = await _userUiService.GetActiveResidents();
            var feeRows = new List<FeeBookRowViewModel>();

            foreach (var user in users)
            {
                var monthlyFee =
                    await _monthlyFeeService.GetMonthlyFeeForUserByMonth(user.Id, DateTime.Now.Month,
                        DateTime.Now.Year);

                var totalIncome = await _userService.GetResidentTotalIncome(user.Id);
                var presentDays = monthlyFee?.PresentDays ?? 0;
                var realCost = monthlyFee?.RealCost ?? 0;
                var feeAmount = monthlyFee?.FeeAmount ?? 0;

                var hasException = await _userService.HasResidentFeeExceptions(user.Id);

                var row = new FeeBookRowViewModel
                {
                    FullName = string.Concat(user.FirstName, " ", user.MiddleName, " ", user.LastName),
                    PresentDays = presentDays,
                    RealCost = realCost,
                    Pension = user.ResidentInfo?.Pension ?? 0.0m,
                    Rent = user.ResidentInfo?.Rent ?? 0.0m,
                    Salary = user.ResidentInfo?.Salary ?? 0.0m,
                    OtherIncome = user.ResidentInfo?.OtherIncome ?? 0.0m,
                    TotalIncome = totalIncome,
                    PercentageType = hasException ? "РИ" : "70%",
                    FeeCalculated = feeAmount,
                    HasFeeException = hasException ? "ДА" : "НЕ"
                };

                feeRows.Add(row);
            }

            var vm = new FeeBookViewModel
            {
                SelectedMonth = DateTime.Now.Month,
                SelectedYear = DateTime.Now.Year,
                Rows = feeRows
            };

            return vm;
        }
    }
}