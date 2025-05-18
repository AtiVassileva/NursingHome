using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly FileUiService _fileUiService;
        private readonly MessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(FileUiService fileUiService, MessageService messageService, IMapper mapper)
        {
            _fileUiService = fileUiService;
            _messageService = messageService;
            _mapper = mapper;
        }

        public IActionResult Create() => View("Form");

        public async Task<IActionResult> Edit(Guid id)
        {
            var message = await _messageService.GetById(id);

            if (message == null || message.AuthorId != User.GetId())
                return Unauthorized();

            var model = _mapper.Map<MessageCreateViewModel>(message);

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            await _fileUiService.CreateMessageWithFile(model, User);

            return LocalRedirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var isEditSuccessful = await _fileUiService.EditMessageWithFile(model, User.GetId());

            if (!isEditSuccessful)
            {
                return Unauthorized();
            }

            return LocalRedirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleteSuccessful = await _messageService.Delete(id, User.GetId());

            if (!isDeleteSuccessful)
            {
                return Unauthorized();
            }

            return LocalRedirect("/");
        }
    }
}