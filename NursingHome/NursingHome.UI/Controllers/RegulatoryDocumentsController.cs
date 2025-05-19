using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class RegulatoryDocumentsController : Controller
    {
        private readonly RegulatoryDocumentService _regulatoryDocumentService;
        private readonly FileUiService _fileUiService;

        public RegulatoryDocumentsController(RegulatoryDocumentService regulatoryDocumentService, FileUiService fileUiService)
        {
            _regulatoryDocumentService = regulatoryDocumentService;
            _fileUiService = fileUiService;
        }
        
        public async Task<IActionResult> Index()
        {
            var documents = await _regulatoryDocumentService.GetAll();

            return View(documents);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegulatoryDocumentViewModel model)
        {
            if (!ModelState.IsValid || model.File == null)
            {
                ModelState.AddModelError("", "Файлът е задължителен!");
                return View(model);
            }

            var isUploadedSuccessfully = await _fileUiService.UploadRegulatoryDocument(User, model);

            if (!isUploadedSuccessfully)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeletedSuccessfully = await _regulatoryDocumentService.Delete(id);

            if (!isDeletedSuccessfully)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}