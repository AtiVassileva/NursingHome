using NursingHome.BLL;
using NursingHome.UI.Models;

namespace NursingHome.UI.Services
{
    public class FeeBookUiService
    {
        private readonly UserService _userService;
        private readonly UserUiService _userUiService;
        private readonly MonthlyParameterService _monthlyParameterService;
        private readonly MonthlyFeeService _monthlyFeeService;

        public FeeBookUiService(UserService userService, UserUiService userUiService, MonthlyParameterService monthlyParameterService, MonthlyFeeService monthlyFeeService)
        {
            _userService = userService;
            _userUiService = userUiService;
            _monthlyParameterService = monthlyParameterService;
            _monthlyFeeService = monthlyFeeService;
        }

        public async Task<FeeBookViewModel> CreateFeeBookViewModel()
        {
            var monthlyParams = await _monthlyParameterService.GetMonthlyParametersByMonth(DateTime.Now.Month,
                DateTime.Now.Year);

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