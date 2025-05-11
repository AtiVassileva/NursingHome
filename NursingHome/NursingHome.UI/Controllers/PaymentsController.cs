using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Models;

namespace NursingHome.UI.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly MonthlyFeeService _monthlyFeeService;
        private readonly PaymentService _paymentService;

        public PaymentsController(MonthlyFeeService monthlyFeeService, PaymentService paymentService)
        {
            _monthlyFeeService = monthlyFeeService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var feesWithPayments = await _monthlyFeeService.GetMonthlyFeesByMonth(DateTime.Now.Month, DateTime.Now.Year);

            var model = new PaymentViewModel
            {
                SelectedMonth = DateTime.Now.Month,
                SelectedYear = DateTime.Now.Year,
                Payments = feesWithPayments.Select(f => new PaymentRowViewModel
                {
                    PaymentId = f.Payment!.Id,
                    ResidentName = $"{f.User!.FirstName} {f.User.MiddleName} {f.User.LastName}",
                    FeeAmount = f.FeeAmount,
                    Status = f.Payment!.Status

                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> MarkAsPaid(Guid id)
        {
            await _paymentService.MarkAsPaid(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkAsUnpaid(Guid id)
        {
            await _paymentService.MarkAsUnpaid(id);
            return RedirectToAction(nameof(Index));
        }
    }
}