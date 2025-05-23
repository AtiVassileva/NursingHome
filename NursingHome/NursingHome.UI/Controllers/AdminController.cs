﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NursingHome.BLL;
using NursingHome.DAL.Models;
using NursingHome.UI.Models.User;
using NursingHome.UI.Services;

namespace NursingHome.UI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserUiService _userUiService;
        private readonly IMapper _mapper;

        public AdminController(UserUiService userUiService, IMapper mapper)
        {
            _userUiService = userUiService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeAccounts()
        {
            var employees = await _userUiService.GetEmployees();
            return View(employees.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> ResidentsAccounts()
        {
            var residents = await _userUiService.GetActiveResidents();
            return View(residents);
        }

        [HttpGet]
        public async Task<IActionResult> GoToArchive()
        {
            var inactiveResidents = await _userUiService.GetInactiveResidents();
            return View("ResidentsArchive", inactiveResidents);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userUiService.GetById(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.ResidentInfo is not null)
            {
                var residentEditModel = _mapper.Map<ResidentEditModel>(user);
                await FetchAvailableEmployees(residentEditModel);

                return View("EditResident", residentEditModel);
            }

            var employeeEditModel = _mapper.Map<EmployeeEditModel>(user);
            return View("EditEmployee", employeeEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResident(string id, ResidentEditModel model)
        {
            if (!ModelState.IsValid)
            {
                await FetchAvailableEmployees(model);

                return View(model);
            }

            var result = await _userUiService.EditResident(id, model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            return RedirectToAction(nameof(ResidentsAccounts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(string id, EmployeeEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userUiService.EditEmployee(id, model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            return RedirectToAction(nameof(EmployeeAccounts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userUiService.DeleteUser(id);

            return result == "resident" 
                ? RedirectToAction(nameof(ResidentsAccounts)) 
                : RedirectToAction(nameof(EmployeeAccounts));
        }

        private async Task FetchAvailableEmployees(ResidentEditModel residentEditModel)
        {
            var employees = await _userUiService.GetEmployees();

            residentEditModel.AvailableEmployees = employees
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.FirstName} {u.LastName}"
                })
                .ToList();
        }
    }
}