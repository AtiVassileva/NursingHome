using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class RoomPlansController : Controller
    {
        private readonly RoomPlanService _roomPlanService;
        private readonly FileUiService _fileUiService;

        public RoomPlansController(RoomPlanService roomPlanService, FileUiService fileUiService)
        {
            _roomPlanService = roomPlanService;
            _fileUiService = fileUiService;
        }

        public async Task<IActionResult> Index()
        {
            var plan = await _roomPlanService.GetLatest();
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Моля, изберете файл!");
                return RedirectToAction(nameof(Index));
            }

            await _fileUiService.UploadRoomPlan(User, file);
            return RedirectToAction(nameof(Index));
        }
    }
}