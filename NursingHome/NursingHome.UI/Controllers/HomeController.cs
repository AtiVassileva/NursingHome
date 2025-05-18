using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Models;
using System.Diagnostics;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Infrastructure;
using static NursingHome.UI.Common.WebConstants;

namespace NursingHome.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageService _messageService;

        public HomeController(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            List<Message> messages;

            if (User.IsInRole(RegularUserRoleName))
            {
                messages = await _messageService.GetMessagesForResidents();
            }
            else
            {
                messages = await _messageService.GetAll();
            }

            return View(messages);
        }

        public IActionResult Contacts() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}