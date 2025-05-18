using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NursingHome.BLL;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class SocialWorkersController : Controller
    {
        private readonly SocialDocumentService _socialDocumentService;
        private readonly UserUiService _userUiService;
        private readonly FileUiService _fileUiService;

        public SocialWorkersController(UserUiService userUiService, FileUiService fileUiService, SocialDocumentService socialDocumentService)
        {
            _userUiService = userUiService;
            _fileUiService = fileUiService;
            _socialDocumentService = socialDocumentService;
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var activeResidents = await _userUiService.GetActiveResidents();

            var model = new UploadSocialDocumentViewModel
            {
                Residents = activeResidents
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = $"{u.FirstName} {u.MiddleName} {u.LastName}"
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var documents = await _socialDocumentService.GetAll();

            var documentModels = documents
            .Select(d => new SocialDocumentViewModel
            {
                Id = d.Id,
                ResidentName = $"{d.Resident.FirstName} {d.Resident.MiddleName} {d.Resident.LastName}",
                FileName = d.FileName,
                FilePath = d.FilePath,
                UploadedOn = d.UploadedOn,
                UploadedBy = d.UploadedBy,
                DocumentType = d.DocumentType
            }).ToList();

            return View(documentModels);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadSocialDocumentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _fileUiService.UploadSocialDocument(User, model);

            return RedirectToAction(nameof(List));
        }
    }
}