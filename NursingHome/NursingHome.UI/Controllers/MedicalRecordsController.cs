using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Infrastructure;
using NursingHome.UI.Models;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly UserUiService _userUiService;
        private readonly MedicalRecordService _medicalRecordService;
        private readonly IMapper _mapper;

        public MedicalRecordsController(UserUiService userUiService, MedicalRecordService medicalRecordService, IMapper mapper)
        {
            _userUiService = userUiService;
            _medicalRecordService = medicalRecordService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userUiService.GetActiveResidents();
            var medicalRecords = await _medicalRecordService.GetAll();

            var viewModel = new AllMedicalRecordsViewModel
            {
                Records = users.Select(user =>
                {
                    var record = medicalRecords.FirstOrDefault(r => r.UserId == user.Id);

                    return new MedicalRecordRowViewModel
                    {
                        UserId = user.Id,
                        FullName = string.Concat(user.FirstName, " ", user.MiddleName, " ", user.LastName),
                        Diet = user.ResidentInfo!.DietNumber.GetDisplayName(),
                        DisabilityPercent = record!.DisabilityPercent,
                        Allergies = record.Allergies,
                        Therapy = record.Therapy
                    };
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var record = await _medicalRecordService.GetMedicalRecordForUser(userId);
            if (record == null) return NotFound();

            var user = await _userUiService.GetById(userId);

            var vm = new MedicalRecordRowViewModel
            {
                UserId = record.UserId,
                DisabilityPercent = record.DisabilityPercent,
                Allergies = record.Allergies,
                Therapy = record.Therapy,
                FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}"
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MedicalRecordRowViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var medicalRecordModel = _mapper.Map<MedicalRecord>(model);
            await _medicalRecordService.UpdateUserMedicalRecord(medicalRecordModel);

            return RedirectToAction(nameof(Index));
        }
    }
}