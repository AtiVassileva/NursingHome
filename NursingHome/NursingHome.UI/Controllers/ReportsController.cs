using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Services;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly FileUiService _filesUiService;
    private readonly ReportService _reportService;

    public ReportsController(FileUiService filesUiService, ReportService reportService)
    {
        _filesUiService = filesUiService;
        _reportService = reportService;
    }

    [HttpGet]
    public IActionResult Upload() => View();

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, ReportType type)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Моля, изберете файл.");
            return View();
        }

        var isReportUploadedSuccessfully = await _filesUiService.UploadReport(file, type, User);

        if (isReportUploadedSuccessfully)
        {
            TempData["Success"] = "Отчетът беше успешно качен!";
        }
        else
        {
            TempData["Error"] = "Възникна проблем при качването на файла! Моля, опитайте отново!";
        }

        return RedirectToAction(nameof(Upload));
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        List<Report> reports = new List<Report>();

        if (User.IsOccupationalTherapist())
        {
            reports = await _reportService.GetOccupationalTherapistsReports();
        }
        else if (User.IsPsychologist())
        {
            reports = await _reportService.GetPsychologistsReports();
        }
        else if (User.IsAdmin())
        {
            reports = await _reportService.GetAll();
        }

        return View(reports);
    }
}