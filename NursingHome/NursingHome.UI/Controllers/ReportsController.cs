using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Services;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly FileUiService _filesUiService;

    public ReportsController(FileUiService filesUiService)
    {
        _filesUiService = filesUiService;
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
        var reports = User.IsOccupationalTherapist()
            ? await _filesUiService.GetOccupationalTherapistsReports()
            : await _filesUiService.GetPsychologistsReports();

        return View(reports);
    }
}