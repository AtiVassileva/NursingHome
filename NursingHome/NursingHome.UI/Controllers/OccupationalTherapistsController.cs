using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class OccupationalTherapistsController : Controller
    {
        private readonly UserUiService _userUiService;
        private readonly IMapper _mapper;

        public OccupationalTherapistsController(UserUiService userUiService, IMapper mapper)
        {
            _userUiService = userUiService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Birthdays()
        {
            var birthdayUsers = await _userUiService.GetBirthdaysThisMonthAsync();
            var birthdayModels = _mapper.Map<List<BirthdayViewModel>>(birthdayUsers);
            return View(birthdayModels);
        }
    }
}