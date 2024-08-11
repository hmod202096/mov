using Appointment.Models;
using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = (SD.SuperAdminUser))]
    public class UserTypesController : Controller
    {
        private readonly IUserTypeReository _userTypeReository;

        public UserTypesController(IUserTypeReository userTypeReository)
        {
            _userTypeReository = userTypeReository;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _userTypeReository.GetAll();

            return View(data);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTypes userTypes)
        {
            if (ModelState.IsValid)
            {
                var sav = await _userTypeReository.AddUserTypes(userTypes);

                if (sav > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(userTypes);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _userTypeReository.GetUserTypes(id);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserTypes userTypes)
        {
            if (ModelState.IsValid)
            {
                var sav = await _userTypeReository.EditUserTypes(userTypes);

                if (sav.Id > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(userTypes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _userTypeReository.GetUserTypes(id);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _userTypeReository.GetUserTypes(id);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserTypes userTypes)
        {
            await _userTypeReository.DeleteUserTypes(userTypes);

            return RedirectToAction(nameof(Index));
        }
    }
}
