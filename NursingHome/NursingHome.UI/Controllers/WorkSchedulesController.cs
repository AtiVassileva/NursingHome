using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Services;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class WorkSchedulesController : Controller
    {
        private readonly FileUiService _fileUiService;
        private readonly WorkScheduleService _workScheduleService;

        public WorkSchedulesController(FileUiService fileUiService, WorkScheduleService workScheduleService)
        {
            _fileUiService = fileUiService;
            _workScheduleService = workScheduleService;
        }

        public async Task<IActionResult> Index(EmployeePosition position)
        {
            var schedules = await _workScheduleService.GetByPosition(position);
            return View("Index", schedules);
        }

        public IActionResult Upload() => View();

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Upload(EmployeePosition position, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Моля, прикачете файл!");
                return View();
            }
            
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Непозволен файлов формат. Разрешени: PDF, DOC, DOCX, JPG, JPEG, PNG.");
                return View();
            }

            var isUploadSuccessful = await _fileUiService.UploadWorkSchedule(position, file);

            if (!isUploadSuccessful)
            {
                return View();
            }

            return RedirectToAction(nameof(Index), new { position });
        }
    }
}