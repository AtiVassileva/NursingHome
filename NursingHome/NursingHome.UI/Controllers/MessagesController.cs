using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly FileUiService _fileUiService;
        private readonly MessageService _messageService;

        public MessagesController(FileUiService fileUiService, MessageService messageService)
        {
            _fileUiService = fileUiService;
            _messageService = messageService;
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Създай съобщение";
            ViewData["Action"] = "Create";
            return View("Form");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["Title"] = "Редактирай съобщение";
            ViewData["Action"] = "Edit";

            var model = await _messageService.GetById(id);
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            await _fileUiService.CreateMessage(model, User);

            return LocalRedirect("/");
        }
    }
}